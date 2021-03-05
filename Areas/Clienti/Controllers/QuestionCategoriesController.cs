using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models.QandA;

namespace Licenta.Areas.Clienti.Views
{
    [Area("Clienti")]
    public class QuestionCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clienti/QuestionCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuestionCategory.ToListAsync());
        }

        // GET: Clienti/QuestionCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionCategory = await _context.QuestionCategory
                .FirstOrDefaultAsync(m => m.QuestionCategoryId == id);
            if (questionCategory == null)
            {
                return NotFound();
            }

            return View(questionCategory);
        }

        // GET: Clienti/QuestionCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clienti/QuestionCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionCategoryId,Denumire")] QuestionCategory questionCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionCategory);
        }

        // GET: Clienti/QuestionCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionCategory = await _context.QuestionCategory.FindAsync(id);
            if (questionCategory == null)
            {
                return NotFound();
            }
            return View(questionCategory);
        }

        // POST: Clienti/QuestionCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionCategoryId,Denumire")] QuestionCategory questionCategory)
        {
            if (id != questionCategory.QuestionCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionCategoryExists(questionCategory.QuestionCategoryId))
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
            return View(questionCategory);
        }

        // GET: Clienti/QuestionCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionCategory = await _context.QuestionCategory
                .FirstOrDefaultAsync(m => m.QuestionCategoryId == id);
            if (questionCategory == null)
            {
                return NotFound();
            }

            return View(questionCategory);
        }

        // POST: Clienti/QuestionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionCategory = await _context.QuestionCategory.FindAsync(id);
            _context.QuestionCategory.Remove(questionCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionCategoryExists(int id)
        {
            return _context.QuestionCategory.Any(e => e.QuestionCategoryId == id);
        }
    }
}
