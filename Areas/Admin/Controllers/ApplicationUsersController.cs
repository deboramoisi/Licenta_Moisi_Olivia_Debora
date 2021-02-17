using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.ViewModels;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers
                                .Include(c => c.Client)
                                .ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // API CALLS
        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.ApplicationUsers.Include(c => c.Client).ToList();
            return Json(new { data = allObj });
        }

        // Id-ul user-ului este de tip string!!!
        [HttpDelete]
        public IActionResult DeleteAPI(string id)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea userului!" });
            }
            else
            {
                _context.Remove(applicationUser);
                _context.SaveChanges();
                return Json(new { success = true, message = "User sters cu succes!" });
            }
        }
        #endregion

    }
}
