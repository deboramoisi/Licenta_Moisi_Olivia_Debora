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
        [HttpGet]
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("_AddQuestionCateg", new QuestionCategory());
            }
            else
            {
                // Pentru return spre Login
                return LocalRedirect("/Identity/Account/Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionCategory qc)
        {
            if (ModelState.IsValid)
            {
                _context.QuestionCategory.Add(qc);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Tip intrebare adaugat cu succes!";
                TempData["Success"] = "true";
            }
            return PartialView("_AddQuestionCateg", qc);
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
                    _context.QuestionCategory.Update(questionCategory);
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
                TempData["Message"] = "Tip intrebare actualizat cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            return View(questionCategory);
        }
        #endregion

        // Delete
        #region
        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            QuestionCategory qc = await _context.QuestionCategory.FindAsync(id);
            if (qc == null)
            {
                return Json(new { success = false, message = "Categoria nu a fost gasita!" });
            }
            else
            {
                _context.QuestionCategory.Remove(qc);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Categorie stearsa cu succes!" });
            }
        }

        private bool QuestionCategoryExists(int id)
        {
            return _context.QuestionCategory.Any(e => e.QuestionCategoryId == id);
        }
        #endregion
    }
}
