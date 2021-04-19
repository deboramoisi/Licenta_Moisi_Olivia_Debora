﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models.Plati;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;

namespace Licenta.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    [Area("Admin")]
    public class PlataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Plati.Include(p => p.Client).Include(p => p.TipPlata);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            ViewData["TipPlataId"] = new SelectList(_context.TipPlati, "TipPlataId", "Denumire");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plata plata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", plata.ClientId);
            ViewData["TipPlataId"] = new SelectList(_context.TipPlati, "TipPlataId", "Denumire", plata.TipPlataId);
            return View(plata);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plata = await _context.Plati.FindAsync(id);
            if (plata == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", plata.ClientId);
            ViewData["TipPlataId"] = new SelectList(_context.TipPlati, "TipPlataId", "Denumire", plata.TipPlataId);
            return View(plata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plata plata)
        {
            if (id != plata.PlataId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlataExists(plata.PlataId))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", plata.ClientId);
            ViewData["TipPlataId"] = new SelectList(_context.TipPlati, "TipPlataId", "Denumire", plata.TipPlataId);
            return View(plata);
        }
        
        private bool PlataExists(int id)
        {
            return _context.Plati.Any(e => e.PlataId == id);
        }

        // API CALLS: get all, delete
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plati = _context.Plati;
            return Json(new { data = await plati
                .Include(x => x.TipPlata)
                .AsNoTracking()
                .ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var plata = _context.Plati.Find(id);
            if (plata == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea platii!" });
            }
            _context.Remove(plata);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Plata a fost stearsa cu succes!" });
        }
        #endregion
    }
}