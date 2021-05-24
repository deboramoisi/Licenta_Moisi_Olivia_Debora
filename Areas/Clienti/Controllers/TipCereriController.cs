using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models.CereriDocumente;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin + " " + ConstantVar.Rol_Admin_Firma)]
    public class TipCereriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipCereriController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TipCereri.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipCerere = await _context.TipCereri
                .FirstOrDefaultAsync(m => m.TipCerereId == id);
            if (tipCerere == null)
            {
                return NotFound();
            }

            return View(tipCerere);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipCerereId,Denumire")] TipCerere tipCerere)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipCerere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipCerere);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipCerere = await _context.TipCereri.FindAsync(id);
            if (tipCerere == null)
            {
                return NotFound();
            }
            return View(tipCerere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipCerereId,Denumire")] TipCerere tipCerere)
        {
            if (id != tipCerere.TipCerereId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipCerere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipCerereExists(tipCerere.TipCerereId))
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
            return View(tipCerere);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipCerere = await _context.TipCereri
                .FirstOrDefaultAsync(m => m.TipCerereId == id);
            if (tipCerere == null)
            {
                return NotFound();
            }

            return View(tipCerere);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipCerere = await _context.TipCereri.FindAsync(id);
            _context.TipCereri.Remove(tipCerere);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipCerereExists(int id)
        {
            return _context.TipCereri.Any(e => e.TipCerereId == id);
        }
    }
}
