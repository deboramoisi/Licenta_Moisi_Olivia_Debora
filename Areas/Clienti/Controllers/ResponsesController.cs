using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models.QandA;
using Licenta.Areas.Clienti.Models;
using Microsoft.AspNetCore.Authorization;

namespace Licenta.Areas.Clienti.Views
{
    [Authorize]
    [Area("Clienti")]
    public class ResponsesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResponsesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index, details
        #region
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Response.Include(r => r.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _context.Response
                .Include(r => r.Question)
                .FirstOrDefaultAsync(m => m.ResponseId == id);
            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }
        #endregion

        // Create and edit
        #region
        public IActionResult Create(int? id)
        {

            Response response = new Response()
            {
                DataAdaugare = DateTime.Now,
            };

            if (id != 0)
            {
                response.QuestionId = id.Value;
                if (_context.Question.Find(id).Descriere != null)
                {
                    ViewBag.Descriere = _context.Question.Find(id).Descriere;
                }
            }

            var question = _context.Question.Find(response.QuestionId);

            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Intrebare", response.QuestionId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResponseId,Raspuns,DataAdaugare,QuestionId")] Response response)
        {
            var question = _context.Question.Find(response.QuestionId);

            if (ModelState.IsValid)
            {
                _context.Add(response);

                question.Rezolvata = true;
                _context.Update(question);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Intrebare", response.QuestionId);
            return View(response);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var response = await _context.Response.FindAsync(id);
            var question = await _context.Question.FindAsync(response.QuestionId);
            //var questionCateg = _context.QuestionCategory.Where(u => u.QuestionCategoryId == question.QuestionCategoryId);

            if (response == null)
            {
                return NotFound();
            }

            var qaVM = new QaVM() { 
                QuestionId = response.QuestionId,
                Raspuns = response.Raspuns,
                DataAdaugare = response.DataAdaugare,
                ResponseId = response.ResponseId,
                QuestionCategoryId = question.QuestionId
            };

            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Intrebare", response.QuestionId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", question.QuestionCategoryId);
            return View(qaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QaVM qaVM)
        {
            var response = new Response
            {
                ResponseId = qaVM.ResponseId,
                DataAdaugare = qaVM.DataAdaugare,
                QuestionId = qaVM.QuestionId,
                Raspuns = qaVM.Raspuns
            };

            var question = await _context.Question.FindAsync(response.QuestionId);

            if (id != response.ResponseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(response);
                    if (question.QuestionCategoryId != qaVM.QuestionCategoryId)
                    {
                        question.QuestionCategoryId = qaVM.QuestionCategoryId;
                        _context.Update(question);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponseExists(response.ResponseId))
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
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Intrebare", response.QuestionId);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategory, "QuestionCategoryId", "Denumire", qaVM.QuestionCategoryId);
            return View(qaVM);
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

            var response = await _context.Response
                .Include(r => r.Question)
                .FirstOrDefaultAsync(m => m.ResponseId == id);
            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _context.Response.FindAsync(id);
            _context.Response.Remove(response);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResponseExists(int id)
        {
            return _context.Response.Any(e => e.ResponseId == id);
        }
#endregion
    }
}
