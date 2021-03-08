using System;
using System.Collections.Generic;
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

        // GET: Clienti/Questions
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

            switch (ViewData["filterBy"].ToString())
            {
                case "All":
                    questions = questions.ToList();
                    break;
                default:
                    questions = questions.Where(q => q.QuestionCategory.Denumire == filterBy).ToList();
                    break;
            }

            // sortare
            switch (sortOrder)
            {
                case "name_desc":
                    questions = questions.OrderByDescending(s => s.Intrebare).ToList();
                    break;
                case "Question":
                    questions = questions.OrderBy(s => s.Intrebare).ToList();
                    break;
                case "Date":
                    questions = questions.OrderBy(s => s.DataAdaugare).ToList();
                    break;
                case "date_desc":
                    questions = questions.OrderByDescending(s => s.DataAdaugare).ToList();
                    break;
                case "Category":
                    questions = questions.OrderBy(s => s.QuestionCategory.Denumire).ToList();
                    break;
                case "Category_desc":
                    questions = questions.OrderByDescending(s => s.QuestionCategory.Denumire).ToList();
                    break;
                default:
                    questions = questions.OrderBy(s => s.Rezolvata).ToList();
                    break;
            }

            // totalitatea categoriilor
            var questionCategories = await _context.QuestionCategory.ToListAsync();
            return View(new QAIndexVM() { 
                questions = questions,
                questionCategories = questionCategories
            });
        }

        // GET: Clienti/Questions/Details/5
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

        // GET: Clienti/Questions/Create
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
                return View(question);
            }
            else
            {
                // Pentru return spre Login
                return LocalRedirect("/Identity/Account/Login");
            }

        }

        // POST: Clienti/Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,Intrebare,DataAdaugare,Rezolvata,QuestionCategoryId,ApplicationUserId")] Question question)
        {
            if (!User.IsInRole(ConstantVar.Rol_Admin))
            {
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
                question.ApplicationUserId = user.Id;
            }

            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", question.ApplicationUserId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return View(question);
        }

        // GET: Clienti/Questions/Edit/5
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

        // POST: Clienti/Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Intrebare,DataAdaugare,Rezolvata,QuestionCategoryId,ApplicationUserId")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Nume", question.ApplicationUserId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return View(question);
        }

        // GET: Clienti/Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.ApplicationUser)
                .Include(q => q.QuestionCategory)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Clienti/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
