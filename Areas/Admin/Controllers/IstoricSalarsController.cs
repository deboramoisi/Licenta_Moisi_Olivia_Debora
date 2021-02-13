using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;

namespace Licenta.Areas.Admin.Views
{
    [Area("Admin")]
    public class IstoricSalarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IstoricSalarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/IstoricSalars
        public async Task<IActionResult> Index()
        {
            return View(await _context.IstoricSalar
                .Include(c => c.Salariat)
                .ToListAsync());
        }

        // GET: Admin/IstoricSalars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var istoricSalar = await _context.IstoricSalar
                .Include(c => c.Salariat)
                .FirstOrDefaultAsync(m => m.IstoricSalarId == id);
            if (istoricSalar == null)
            {
                return NotFound();
            }

            return View(istoricSalar);
        }

        // GET: Admin/IstoricSalars/Create
        public IActionResult Create()
        {
            ViewData["SalariatId"] = new SelectList(_context.Salariat, "SalariatId", "NumePrenume");
            return View();
        }

        // POST: Admin/IstoricSalars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IstoricSalarId,SalariatId,DataInceput,DataSfarsit,Salariu")] IstoricSalar istoricSalar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(istoricSalar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalariatId"] = new SelectList(_context.Salariat, "SalariatId", "NumePrenume");
            return View(istoricSalar);
        }

        // GET: Admin/IstoricSalars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var istoricSalar = await _context.IstoricSalar.FindAsync(id);
            if (istoricSalar == null)
            {
                return NotFound();
            }
            ViewData["SalariatId"] = new SelectList(_context.Salariat, "SalariatId", "NumePrenume", istoricSalar.SalariatId);
            return View(istoricSalar);
        }

        // POST: Admin/IstoricSalars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IstoricSalarId,SalariatId,DataInceput,DataSfarsit,Salariu")] IstoricSalar istoricSalar)
        {
            if (id != istoricSalar.IstoricSalarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(istoricSalar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IstoricSalarExists(istoricSalar.IstoricSalarId))
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
            ViewData["SalariatId"] = new SelectList(_context.Salariat, "SalariatId", "NumePrenume", istoricSalar.SalariatId);
            return View(istoricSalar);
        }

        // GET: Admin/IstoricSalars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var istoricSalar = await _context.IstoricSalar
                .FirstOrDefaultAsync(m => m.IstoricSalarId == id);
            if (istoricSalar == null)
            {
                return NotFound();
            }

            return View(istoricSalar);
        }

        // POST: Admin/IstoricSalars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var istoricSalar = await _context.IstoricSalar.FindAsync(id);
            _context.IstoricSalar.Remove(istoricSalar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IstoricSalarExists(int id)
        {
            return _context.IstoricSalar.Any(e => e.IstoricSalarId == id);
        }
    }
}
