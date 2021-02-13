using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalariatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalariatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Salariats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Salariat.Include(c => c.Client).ToListAsync());
        }

        // GET: Admin/Salariats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salariat = await _context.Salariat
                .FirstOrDefaultAsync(m => m.SalariatId == id);
            if (salariat == null)
            {
                return NotFound();
            }

            return View(salariat);
        }

        // GET: Admin/Salariats/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View();
        }

        // POST: Admin/Salariats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalariatId,Nume,Prenume,Pozitie,DataAngajare,DataConcediere,ClientId")] Salariat salariat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salariat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View(salariat);
        }

        // GET: Admin/Salariats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salariat = await _context.Salariat.FindAsync(id);
            if (salariat == null)
            {
                return NotFound();
            }
            return View(salariat);
        }

        // POST: Admin/Salariats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalariatId,Nume,Prenume,Pozitie,DataAngajare,DataConcediere")] Salariat salariat)
        {
            if (id != salariat.SalariatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salariat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalariatExists(salariat.SalariatId))
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
            return View(salariat);
        }

        // GET: Admin/Salariats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salariat = await _context.Salariat
                .FirstOrDefaultAsync(m => m.SalariatId == id);
            if (salariat == null)
            {
                return NotFound();
            }

            return View(salariat);
        }

        // POST: Admin/Salariats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salariat = await _context.Salariat.FindAsync(id);
            _context.Salariat.Remove(salariat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalariatExists(int id)
        {
            return _context.Salariat.Any(e => e.SalariatId == id);
        }
    }
}
