using System;
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
using System.Xml.Linq;
using Licenta.Areas.Admin.Models;
using System.Xml;

namespace Licenta.Areas.Admin.Views
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly UserManager<IdentityUser> _userManager;

        public DocumentsController(ApplicationDbContext context, IFileManager fileManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        // GET: Admin/Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Documents/IncarcariUtilizatori
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

        // GET: Admin/Documents/Details/5
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

        [HttpGet]
        // GET: Admin/Documents/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(u => u.Nume), "Id", "Nume");
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument.OrderBy(u => u.Denumire), "TipDocumentId", "Denumire");
            return View();
        }

        // POST: Admin/Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentVM documentVM)
        {
            var denumireDocument = _context.TipDocument.Find(documentVM.TipDocumentId);

            Console.WriteLine(documentVM.DocumentPathUrl);

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
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", document.ClientId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", document.ApplicationUserId);
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
        }

        // GET: Admin/Documents/Edit/5
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

        // POST: Admin/Documents/Edit/5
        // [Bind("DocumentId,DocumentPath,ClientId,ApplicationUserId,TipDocumentId")]
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
                    _context.Update(document);
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

        // API CALLS
        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.Document.Include(d => d.TipDocument).Include(d => d.Client).Include(d => d.ApplicationUser).OrderBy(u => u.Client.Denumire).ToList();
            return Json(new { data = allObj });
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
        public IActionResult DeleteAPI(int id)
        {
            Document document = _context.Document.Find(id);
            if (document == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea documentului!" });
            }
            else
            {
                _context.Document.Remove(document);
                _context.SaveChanges();
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

        // XML parsing
        #region
        public IActionResult XMLParsingExample()
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            XDocument doc = XDocument.Load("C:/Users/user/source/repos/Licenta/wwwroot/xml/Customers.xml");

            // XmlDocument doc = new XmlDocument();
            // doc.Load("C:/Users/user/source/repos/Licenta/wwwroot/xml/Customers.xml");

            var q = from el in doc.Root.Elements()
                    select el;

            foreach (XElement e in q) {
                customers.Add(new CustomerModel {
                    CustomerId = int.Parse(e.Element("Id").Value),
                    Name = e.Element("Name").Value.ToString(),
                    Country = e.Element("Country").Value.ToString()
                });
            }

            //foreach (XmlNode node in doc.SelectNodes("/Customers/Customer"))
            //{
            //    customers.Add(new CustomerModel {
            //        CustomerId = int.Parse(node["Id"].InnerText),
            //        Name = node["Name"].InnerText,
            //        Country = node["Country"].InnerText
            //    });
            //}
            return View(customers);
        }
        #endregion
    }
}
