using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;

namespace Licenta.Views.Furnizors
{
    public class FurnizorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FurnizorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Furnizors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Furnizor.ToListAsync());
        }

        // GET: Furnizors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnizor = await _context.Furnizor
                .FirstOrDefaultAsync(m => m.FurnizorID == id);
            if (furnizor == null)
            {
                return NotFound();
            }

            return View(furnizor);
        }

        // GET: Furnizors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Furnizors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FurnizorID,Denumire")] Furnizor furnizor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(furnizor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(furnizor);
        }

        // GET: Furnizors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnizor = await _context.Furnizor.FindAsync(id);
            if (furnizor == null)
            {
                return NotFound();
            }
            return View(furnizor);
        }

        // POST: Furnizors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FurnizorID,Denumire")] Furnizor furnizor)
        {
            if (id != furnizor.FurnizorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(furnizor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurnizorExists(furnizor.FurnizorID))
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
            return View(furnizor);
        }

        // GET: Furnizors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnizor = await _context.Furnizor
                .FirstOrDefaultAsync(m => m.FurnizorID == id);
            if (furnizor == null)
            {
                return NotFound();
            }

            return View(furnizor);
        }

        // POST: Furnizors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var furnizor = await _context.Furnizor.FindAsync(id);
            _context.Furnizor.Remove(furnizor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurnizorExists(int id)
        {
            return _context.Furnizor.Any(e => e.FurnizorID == id);
        }
    }
}
