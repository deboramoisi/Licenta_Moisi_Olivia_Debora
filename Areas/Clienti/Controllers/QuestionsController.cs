using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models.QandA;
using Licenta.Utility;
using Licenta.ViewModels;

namespace Licenta.Areas.Clienti.Views
{
    [Area("Clienti")]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index - sortare, filtrare, cautare
        #region
        public async Task<IActionResult> Index(string sortOrder, string filterBy, string searchQuestion)
        {
            // Daca sortOrder == Question, atunci aveam sortare crescatoare, deci trecem pe descrescatoare
            // Altfel ii atribuim sortarea crescatoare
            ViewData["QuestionSortParam"] = sortOrder == "Question" ? "name_desc" : "Question";
            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CategorySortParam"] = sortOrder == "Category" ? "category_desc" : "Category";
            ViewData["CurrentSearch"] = searchQuestion;

            var questions = _context.Question
                .Include(q => q.QuestionCategory)
                .Include(q => q.ApplicationUser)
                .ToList();

            if (!String.IsNullOrEmpty(searchQuestion))
            {
                questions = questions.Where(q => q.Intrebare.ToLower().Contains(searchQuestion.ToLower())).ToList();
            }

            // Filtrare in functie de categorie
            ViewData["filterBy"] = (String.IsNullOrEmpty(filterBy)) ? "All" : filterBy;

            questions = (ViewData["filterBy"].ToString()) switch
            {
                "All" => questions.ToList(),
                _ => questions.Where(q => q.QuestionCategory.Denumire == filterBy).ToList(),
            };

            // sortare
            questions = sortOrder switch
            {
                "name_desc" => questions.OrderByDescending(s => s.Intrebare).ToList(),
                "Question" => questions.OrderBy(s => s.Intrebare).ToList(),
                "Date" => questions.OrderBy(s => s.DataAdaugare).ToList(),
                "date_desc" => questions.OrderByDescending(s => s.DataAdaugare).ToList(),
                "Category" => questions.OrderBy(s => s.QuestionCategory.Denumire).ToList(),
                "Category_desc" => questions.OrderByDescending(s => s.QuestionCategory.Denumire).ToList(),
                _ => questions.OrderBy(s => s.Rezolvata).ToList(),
            };

            // totalitatea categoriilor
            var questionCategories = await _context.QuestionCategory.ToListAsync();
            return View(new QAIndexVM()
            {
                questions = questions,
                questionCategories = questionCategories
            });
        }
        #endregion

        // Details
        #region
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.ApplicationUser)
                .Include(q => q.QuestionCategory)
                .Include(q => q.Responses)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        #endregion

        // Create Question
        #region
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                Question question = new Question()
                {
                    DataAdaugare = DateTime.Now
                };
                ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume");
                ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire");
                return PartialView("_AddQuestion", question);
            }
            else
            {
                // Pentru return spre Login
                return LocalRedirect("/Identity/Account/Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question)
        {
            if (!User.IsInRole(ConstantVar.Rol_Admin))
            {
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
                question.ApplicationUserId = user.Id;
            }

            if (ModelState.IsValid)
            {
                _context.Question.Add(question);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Intrebare adaugata cu succes!";
                TempData["Success"] = "true";
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", question.ApplicationUserId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return PartialView("_AddQuestion", question);
        }
        #endregion

        // Edit question
        #region
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", question.ApplicationUserId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    question.Rezolvata = question.Responses != null;
                    _context.Question.Update(question);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Intrebare actualizata cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", question.ApplicationUserId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> QuestionsByCategory(int category)
        {
            var questions = await _context.Question
                .Where(u => u.QuestionCategoryId == category)
                .Include(u => u.ApplicationUser)
                .Include(u => u.QuestionCategory)
                .AsNoTracking()
                .ToListAsync();
            return View(questions);
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
        #endregion

        // API CALLS: delete
        #region
        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            Question question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return Json(new { success = false, message = "Intrebarea nu a fost gasita!" });
            }
            else
            {
                _context.Question.Remove(question);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Intrebare stearsa cu succes!" });
            }
        }
        #endregion
    }
}
