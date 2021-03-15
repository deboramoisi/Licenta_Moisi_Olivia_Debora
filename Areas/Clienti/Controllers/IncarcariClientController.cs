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

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin_Firma)]
    public class IncarcariClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public IncarcariClientController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
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
                ClientId = userClientId,
                ApplicationUserId = user.Id
            };

            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire");
            return View(documentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentVM documentVM)
        {
            var denumireDocument = _context.TipDocument.Find(documentVM.TipDocumentId);

            Console.WriteLine(documentVM.DocumentPathUrl);

            Document document = new Document()
            {
                TipDocumentId = documentVM.TipDocumentId,
                DocumentPath = await _fileManager.SaveDocument(documentVM.DocumentPathUrl, denumireDocument.Denumire, documentVM.ClientId, documentVM.ApplicationUserId)
            };

            document.ApplicationUserId = documentVM.ApplicationUserId;
            document.ClientId = documentVM.ClientId;

            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
        }
        #endregion

    }
}
