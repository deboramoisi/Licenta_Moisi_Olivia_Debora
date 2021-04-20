using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models.Plati;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;

namespace Licenta.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    [Area("Admin")]
    public class TipPlataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipPlataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TipPlati
                .OrderBy(x => x.Denumire)
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_AddTipPlata", new TipPlata { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipPlata tipPlata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipPlata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_AddTipPlata", tipPlata);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipPlata = await _context.TipPlati.FindAsync(id);
            if (tipPlata == null)
            {
                return NotFound();
            }
            return PartialView("_EditTipPlata", tipPlata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipPlata tipPlata)
        {
            if (id != tipPlata.TipPlataId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipPlata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipPlataExists(tipPlata.TipPlataId))
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
            return PartialView("_EditTipPlata", tipPlata);
        }

        private bool TipPlataExists(int id)
        {
            return _context.TipPlati.Any(e => e.TipPlataId == id);
        }

        // API CALLS: get all, delete
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tipPlati = _context.TipPlati;
            return Json(new { data = await tipPlati.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var tipPlata = _context.TipPlati.Find(id);
            if (tipPlata == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea tipului!" });
            }
            _context.Remove(tipPlata);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Tipul platii a fost sters cu succes!" });
        }
        #endregion
    }
}
