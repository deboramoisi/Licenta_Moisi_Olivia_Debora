using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.ViewModels;
using Licenta.Services.FileManager;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;
using Microsoft.AspNetCore.Identity;
using Licenta.Models.Notificari;
using Licenta.Services.NotificationManager;
using System;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationManager _notificationManager;

        public DocumentsController(ApplicationDbContext context,
            IFileManager fileManager,
            UserManager<ApplicationUser> userManager,
            INotificationManager notificationManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
            _notificationManager = notificationManager;
        }

        // Index, Details
        #region
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Client)
                .Include(d => d.TipDocument)
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }
        #endregion

        // Incarcari utilizator, Admin or not
        #region
        public async Task<IActionResult> IncarcariUtilizatori()
        {
            var applicationDbContext = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument);
            return View(await applicationDbContext.ToListAsync());
        }

        private async Task<bool> AdminOrNot(string id)
        {
            var user = _context.ApplicationUsers.Find(id);
            if (await _userManager.IsInRoleAsync(user, ConstantVar.Rol_Admin))
            {
                return false;
            }
            return true;
        }
        #endregion

        // Create, import new document
        #region
        [HttpGet]
        public IActionResult Create(int? id)
        {
            if (id != 0)
            {
                // completam campurile pe care le stim pentru ca am fost redirectati dinspre "Incarca document" - modul cerere documente
                var cerereDocument = _context.CereriDocumente.FirstOrDefault(x => x.CerereDocumentId == id);
                var client = _context.Client.FirstOrDefault(x => x.Denumire == cerereDocument.DenumireClient).ClientId;
                var admin = _context.ApplicationUsers.FirstOrDefault(x => x.Email.Contains("dana_moisi")).Id;
                var tipDocument = _context.TipDocument.FirstOrDefault(x => x.Denumire.Equals(cerereDocument.DenumireCerere)).TipDocumentId;
                ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire", client);
                ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(u => u.Nume), "Id", "Nume", admin);
                ViewData["TipDocumentId"] = new SelectList(_context.TipDocument.OrderBy(u => u.Denumire), "TipDocumentId", "Denumire", tipDocument);
            }
            else
            {
                ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
                ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(u => u.Nume), "Id", "Nume");
                ViewData["TipDocumentId"] = new SelectList(_context.TipDocument.OrderBy(u => u.Denumire), "TipDocumentId", "Denumire");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentVM documentVM, int? id)
        {
            var denumireDocument = _context.TipDocument.Find(documentVM.TipDocumentId);

            Document document = new Document()
            {
                DocumentId = documentVM.DocumentId,
                ApplicationUserId = documentVM.ApplicationUserId,
                ClientId = documentVM.ClientId,
                TipDocumentId = documentVM.TipDocumentId,
                DocumentPath = await _fileManager.SaveDocument(documentVM.DocumentPathUrl, denumireDocument.Denumire, documentVM.ClientId, documentVM.ApplicationUserId),
                Data = documentVM.Data
            };

            if (ModelState.IsValid)
            {
                _context.Document.Add(document);
                await _context.SaveChangesAsync();
                // aici notificam user-ul ca a primit un document
                var notificare = new Notificare();

                if (id != 0)
                {
                    // atunci am fost redirectati dinspre "Incarca document" - modul cereri documente
                    var cerereDocument = _context.CereriDocumente.FindAsync(id).Result;
                    var salariat = _context.Salariat.FirstOrDefault(x => x.SalariatId == cerereDocument.SalariatId);
                    notificare.Text = "Cererea pentru " + cerereDocument.DenumireCerere + " pentru firma " + cerereDocument.DenumireClient + ", salariatul "
                                        + salariat.NumePrenume + " deadline la " + cerereDocument.DataStart + " a fost incarcata " + DateTime.Now;
                    await _notificationManager.CreateAsync(notificare, _context.Client.FirstOrDefault(x => x.Denumire == cerereDocument.DenumireClient).ClientId);
                    
                    cerereDocument.Resolved = true;
                    cerereDocument.AdeverintaPath = document.DocumentPath;
                    _context.CereriDocumente.Update(cerereDocument);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    notificare.Text = $"{document.ApplicationUser.Nume} a adaugat {document.TipDocument.Denumire} - data {document.Data}";
                    // notificare
                    await _notificationManager.CreateAsync(notificare, document.ClientId);
                }
                TempData["Message"] = "Document adaugat cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", document.ClientId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", document.ApplicationUserId);
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
        }
        #endregion

        // Edit, edit uploaded document
        #region
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var documentVM = new DocumentVM()
            {
                DocumentPathUrl = null,
                DocumentId = document.DocumentId,
                TipDocumentId = document.TipDocumentId,
                ClientId = document.ClientId,
                ApplicationUserId = document.ApplicationUserId,
                Data = document.Data
            };

            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", document.ClientId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", document.ApplicationUserId);
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentVM documentVM)
        {
            var document = await _context.Document.FindAsync(documentVM.DocumentId);

            if (id != document.DocumentId)
            {
                return NotFound();
            }

            if (documentVM.DocumentPathUrl != null)
            {
                var documentType = _context.TipDocument.Find(documentVM.TipDocumentId);
                document.DocumentPath = await _fileManager.UpdateDocument(documentVM.DocumentPathUrl, documentVM.DocumentId, documentType.Denumire, documentVM.ClientId, documentVM.ApplicationUserId);
            } 

            if (ModelState.IsValid)
            {
                try
                {
                    document.ApplicationUserId = documentVM.ApplicationUserId;
                    document.ClientId = documentVM.ClientId;
                    document.TipDocumentId = documentVM.TipDocumentId;
                    document.Data = documentVM.Data;
                    _context.Document.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Document actualizat cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", document.ClientId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", document.ApplicationUserId);
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.DocumentId == id);
        }
        #endregion

        // API CALLS
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = _context.Document
                .Include(d => d.TipDocument)
                .Include(d => d.Client)
                .Include(d => d.ApplicationUser)
                .OrderBy(u => u.Client.Denumire);
            return Json(new { data = await allObj.ToListAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var allDocuments = _context.Document.Include(d => d.ApplicationUser).Include(d => d.TipDocument).Include(d => d.Client).OrderBy(u => u.ApplicationUser.Nume);
            var myList = new List<Document>();
            foreach (var doc in allDocuments)
            {
                if (await _userManager.IsInRoleAsync(doc.ApplicationUser, ConstantVar.Rol_Admin_Firma) || await _userManager.IsInRoleAsync(doc.ApplicationUser, ConstantVar.Rol_User_Individual))
                {
                    myList.Add(doc);
                }
            }
            return Json(new { data = myList });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            Document document = _context.Document.Find(id);
            if (document == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea documentului!" });
            }
            else
            {
                _context.Document.Remove(document);
                await _context.SaveChangesAsync();
                if (_fileManager.DeleteDocument(document.DocumentPath) == "Success")
                {
                    return Json(new { success = true, message = "Document sters cu succes!" });
                } else
                {
                    return Json(new { success = false, message = "Eroare la stergerea documentului!" });
                }
            }
        }
        #endregion
    }
}
