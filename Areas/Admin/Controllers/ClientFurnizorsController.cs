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
    public class ClientFurnizorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientFurnizorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClientFurnizors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClientFurnizor.Include(c => c.Client).Include(c => c.Furnizor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClientFurnizors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientFurnizor = await _context.ClientFurnizor
                .Include(c => c.Client)
                .Include(c => c.Furnizor)
                .FirstOrDefaultAsync(m => m.ClientFurnizorId == id);
            if (clientFurnizor == null)
            {
                return NotFound();
            }

            return View(clientFurnizor);
        }

        // GET: ClientFurnizors/Create
        public IActionResult Create()
        {
            // Select List Denumire Client
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            // Select List Denumire Furnizor
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire");
            return View();
        }

        // POST: ClientFurnizors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientFurnizorId,ClientId,FurnizorId")] ClientFurnizor clientFurnizor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientFurnizor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", clientFurnizor.ClientId);
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire", clientFurnizor.FurnizorId);
            return View(clientFurnizor);
        }

        // GET: ClientFurnizors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientFurnizor = await _context.ClientFurnizor.FindAsync(id);
            if (clientFurnizor == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", clientFurnizor.ClientId);
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire", clientFurnizor.FurnizorId);
            return View(clientFurnizor);
        }

        // POST: ClientFurnizors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientFurnizorId,ClientId,FurnizorId")] ClientFurnizor clientFurnizor)
        {
            if (id != clientFurnizor.ClientFurnizorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientFurnizor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientFurnizorExists(clientFurnizor.ClientFurnizorId))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", clientFurnizor.ClientId);
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire", clientFurnizor.FurnizorId);
            return View(clientFurnizor);
        }

        // GET: ClientFurnizors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientFurnizor = await _context.ClientFurnizor
                .Include(c => c.Client)
                .Include(c => c.Furnizor)
                .FirstOrDefaultAsync(m => m.ClientFurnizorId == id);
            if (clientFurnizor == null)
            {
                return NotFound();
            }

            return View(clientFurnizor);
        }

        // POST: ClientFurnizors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientFurnizor = await _context.ClientFurnizor.FindAsync(id);
            _context.ClientFurnizor.Remove(clientFurnizor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientFurnizorExists(int id)
        {
            return _context.ClientFurnizor.Any(e => e.ClientFurnizorId == id);
        }

        // API CALLS
        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var clientFurnizors = _context.ClientFurnizor.Include(c => c.Client).Include(c => c.Furnizor).ToList();
            return Json(new { data = clientFurnizors });
        }

        [HttpDelete]
        public IActionResult DeleteAPI(int id)
        {
            ClientFurnizor clientFurnizor = _context.ClientFurnizor.Find(id);
            if (clientFurnizor == null)
            {
                return Json(new { success = false, message = "Eroare la stergere!"});
            } 
            else {
                _context.Remove(clientFurnizor);
                _context.SaveChanges();
                return Json(new { success = true, message = "Stergere realizata cu succes!" });
            }
        }
        #endregion
    }
}
