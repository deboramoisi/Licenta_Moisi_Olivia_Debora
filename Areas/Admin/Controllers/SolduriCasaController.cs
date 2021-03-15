using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Licenta.Services.FileManager;

namespace Licenta.Areas.Admin.Views
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]

    public class SolduriCasaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public SolduriCasaController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // Details, Index
        #region
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SolduriCasa.Include(s => s.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solduriCasa = await _context.SolduriCasa
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.data == id);
            if (solduriCasa == null)
            {
                return NotFound();
            }

            return View(solduriCasa);
        }
        #endregion

        // Create and Edit
        #region
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolduriCasa solduriCasa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solduriCasa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", solduriCasa.ClientId);
            return View(solduriCasa);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var solduriCasa = await _context.SolduriCasa.FindAsync(id);
            if (solduriCasa == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", solduriCasa.ClientId);
            return View(solduriCasa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SolduriCasa solduriCasa)
        {
            if (id != solduriCasa.SolduriCasaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solduriCasa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolduriCasaExists(solduriCasa.SolduriCasaId))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", solduriCasa.ClientId);
            return View(solduriCasa);
        }

        private bool SolduriCasaExists(int id)
        {
            return _context.SolduriCasa.Any(e => e.SolduriCasaId == id);
        }
        #endregion

        // API CALLS: get all, delete
        #region
        [HttpGet]
        public IActionResult GetAllSolduri()
        {
            var solduri = _context.SolduriCasa.Include(u => u.Client).OrderBy(u => u.ClientId).ThenByDescending(u => u.data);
            return Json(new { data = solduri.ToList() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var sold = _context.SolduriCasa.FirstOrDefault(u => u.SolduriCasaId == id);

            if (id == 0 || sold == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea soldului!" });
            } 
            else
            {
                _context.Remove(sold);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Sold sters cu succes!" });
            }

        }
        #endregion
    }
}
