using Licenta.Data;
using Licenta.Services.FileManager;
using Licenta.Utility;
using Licenta.Models;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Models.Notificari;
using Licenta.Services.NotificationManager;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin_Firma)]
    public class IncarcariClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly INotificationManager _notificationManager;

        public IncarcariClientController(ApplicationDbContext context, 
            IFileManager fileManager,
            INotificationManager notificationManager)
        {
            _context = context;
            _fileManager = fileManager;
            _notificationManager = notificationManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = _context.ApplicationUsers.FirstOrDefault(d => d.UserName == User.Identity.Name);
            var applicationDbContext = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument).Where(d => d.ClientId == user.ClientId && d.ApplicationUserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // Create - incarcare document
        #region
        public IActionResult Create()
        {
            var user = _context.ApplicationUsers.FirstOrDefault(d => d.UserName == User.Identity.Name);

            var userClientId = 0;

            if (user.ClientId != 0)
            {
                userClientId = user.ClientId.Value;
            }

            DocumentVM documentVM = new DocumentVM()
            {
                ClientId = user.ClientId.Value,
                ApplicationUserId = user.Id
            };
            ViewData["UserId"] = user.UserName;
            ViewData["ClientId"] = _context.Client.Find(userClientId).Denumire;
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire");
            return View(documentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentVM documentVM)
        {
            var denumireDocument = _context.TipDocument.Find(documentVM.TipDocumentId);
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Console.WriteLine(documentVM.DocumentPathUrl);

            Document document = new Document()
            {
                DocumentId = documentVM.DocumentId,
                ApplicationUserId = user.Id,
                Data = DateTime.Now,
                ClientId = user.ClientId.Value,
                TipDocumentId = documentVM.TipDocumentId,
                DocumentPath = await _fileManager.SaveDocument(documentVM.DocumentPathUrl, denumireDocument.Denumire, user.ClientId.Value, user.Id)
            };

            if (ModelState.IsValid)
            {
                _context.Document.Add(document);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Document transmis cu succes!";
                TempData["Success"] = "true";

                var admin = _context.ApplicationUsers.FirstOrDefault(x => x.Email.Contains("dana_moisi")).Id;

                Notificare notificare = new Notificare();
                string redirectToPage = "https://localhost:5001/img/documente/" + document.DocumentPath;
                notificare.Text = $"{User.Identity.Name} a incarcat {denumireDocument.Denumire} pentru {document.Data} in {DateTime.Now}";
                notificare.RedirectToPage = redirectToPage;
                await _notificationManager.CreateAsyncNotificationForAdmin(notificare, admin);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = user.Nume;
            ViewData["ClientId"] = _context.Client.Find(document.ClientId).Denumire;
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
        }
        #endregion


        // API CALLS
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = _context.ApplicationUsers.FirstOrDefault(d => d.UserName == User.Identity.Name);
            var applicationDbContext = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument).Where(d => d.ClientId == user.ClientId && d.ApplicationUserId == user.Id);
            return Json(new { data = await applicationDbContext.ToListAsync() });
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
                }
                else
                {
                    return Json(new { success = false, message = "Eroare la stergerea documentului!" });
                }
            }
        }
        #endregion

    }
}
