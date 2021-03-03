using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]

    public class SediuSocialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SediuSocialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SediuSocials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SediuSocial.Include(s => s.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SediuSocials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sediuSocial = await _context.SediuSocial
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.SediuSocialId == id);
            if (sediuSocial == null)
            {
                return NotFound();
            }

            return View(sediuSocial);
        }

        // GET: SediuSocials/Create
        public IActionResult Create()
        {
            ViewData["SediuSocialId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View();
        }

        // POST: SediuSocials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SediuSocialId,Localitate,Judet,Sector,Strada,Numar,CodPostal,Bl,Sc,Et,Ap,Telefon,Email")] SediuSocial sediuSocial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sediuSocial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SediuSocialId"] = new SelectList(_context.Client, "ClientId", "Denumire", sediuSocial.SediuSocialId);
            return View(sediuSocial);
        }

        // GET: SediuSocials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sediuSocial = await _context.SediuSocial.FindAsync(id);
            if (sediuSocial == null)
            {
                return NotFound();
            }
            ViewData["SediuSocialId"] = new SelectList(_context.Client, "ClientId", "Denumire", sediuSocial.SediuSocialId);
            return View(sediuSocial);
        }

        // POST: SediuSocials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SediuSocialId,Localitate,Judet,Sector,Strada,Numar,CodPostal,Bl,Sc,Et,Ap,Telefon,Email")] SediuSocial sediuSocial)
        {
            if (id != sediuSocial.SediuSocialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sediuSocial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SediuSocialExists(sediuSocial.SediuSocialId))
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
            ViewData["SediuSocialId"] = new SelectList(_context.Client, "ClientId", "Denumire", sediuSocial.SediuSocialId);
            return View(sediuSocial);
        }

        private bool SediuSocialExists(int id)
        {
            return _context.SediuSocial.Any(e => e.SediuSocialId == id);
        }

        // API CALLS
        #region
        public IActionResult GetAll()
        {
            var sediiSociale = _context.SediuSocial.Include(c => c.Client).ToList();
            return Json(new { data = sediiSociale });
        }

        [HttpDelete]
        public IActionResult DeleteAPI(int id)
        {
            SediuSocial sediuSocial = _context.SediuSocial.Find(id);
            if(sediuSocial == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea sediului social!" });
            } 
            else
            {
                _context.Remove(sediuSocial);
                _context.SaveChanges();
                return Json(new { success = true, message = "Sediu social sters cu succes!" });
            }
        }

        #endregion
    }
}
