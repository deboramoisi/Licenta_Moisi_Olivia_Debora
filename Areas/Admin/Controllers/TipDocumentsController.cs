using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;

namespace Licenta.Areas.Admin.Views
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class TipDocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TipDocument.ToListAsync());
        }

        // Create si Edit
        #region
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipDocumentId,Denumire")] TipDocument tipDocument)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(tipDocument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipDocument);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipDocument = await _context.TipDocument.FindAsync(id);
            if (tipDocument == null)
            {
                return NotFound();
            }
            return View(tipDocument);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipDocumentId,Denumire")] TipDocument tipDocument)
        {
            if (id != tipDocument.TipDocumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipDocumentExists(tipDocument.TipDocumentId))
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
            return View(tipDocument);
        }
        
        private bool TipDocumentExists(int id)
        {
            return _context.TipDocument.Any(e => e.TipDocumentId == id);
        }
        #endregion

        // API CALLS: get all, delete
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tipDocuments = _context.TipDocument;
            return Json(new { data = await tipDocuments.ToListAsync() });
        }

        // apel api folosind metoda delete din js
        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var tipDocument = _context.TipDocument.Find(id);
            if (tipDocument == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea tipului!" });
            }
            _context.Remove(tipDocument);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Tipul documentului a fost sters cu succes!" });
        }
        #endregion
    }
}
