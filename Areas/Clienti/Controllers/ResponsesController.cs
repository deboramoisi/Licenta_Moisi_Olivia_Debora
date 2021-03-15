using System;
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

            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Intrebare");
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
            if (response == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Intrebare", response.QuestionId);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResponseId,Raspuns,DataAdaugare,QuestionId")] Response response)
        {
            if (id != response.ResponseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(response);
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
            return View(response);
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
