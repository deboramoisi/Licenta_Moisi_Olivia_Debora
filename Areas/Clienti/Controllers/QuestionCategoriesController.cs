using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // Index, Details
        #region
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuestionCategory.OrderBy(u => u.Denumire).ToListAsync());
        }

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
        #endregion

        // Create_Edit
        #region
        public IActionResult Create()
        {
            return View();
        }

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
        #endregion

        // Delete
        #region
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
        #endregion
    }
}
