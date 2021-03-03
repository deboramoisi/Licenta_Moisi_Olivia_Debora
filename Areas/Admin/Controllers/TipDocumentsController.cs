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

        // GET: Admin/TipDocuments
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipDocument.ToListAsync());
        }

        // GET: Admin/TipDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipDocument = await _context.TipDocument
                .FirstOrDefaultAsync(m => m.TipDocumentId == id);
            if (tipDocument == null)
            {
                return NotFound();
            }

            return View(tipDocument);
        }

        // GET: Admin/TipDocuments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TipDocuments/Create
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

        // GET: Admin/TipDocuments/Edit/5
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

        // POST: Admin/TipDocuments/Edit/5
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
    }
}
