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

namespace Licenta.Areas.Admin.Views
{
    [Area("Admin")]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public DocumentsController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // GET: Admin/Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument);
            return View(await applicationDbContext.ToListAsync());
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

        // GET: Admin/Documents/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume");
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument, "TipDocumentId", "Denumire");
            return View();
        }

        // POST: Admin/Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                DocumentPath = await _fileManager.SaveDocument(documentVM.DocumentPathUrl, denumireDocument.Denumire, documentVM.ClientId, documentVM.ApplicationUserId)
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
                ApplicationUserId = document.ApplicationUserId
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
                    document.TipDocumentId = document.TipDocumentId;
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

        // GET: Admin/Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            if (_fileManager.DeleteDocument(document.DocumentPath) == "Success")
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.DocumentId == id);
        }
    }
}
