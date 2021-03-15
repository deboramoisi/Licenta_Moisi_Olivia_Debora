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
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;
using Microsoft.AspNetCore.Identity;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Index, Details
        #region
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers
                                .Include(c => c.Client)
                                .OrderBy(u => u.Nume)
                                .ToListAsync());
        }

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
        #endregion

        // Edit
        #region 
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client.ToList(), "ClientId", "Denumire");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var applicationUser = _context.ApplicationUsers.Find(id);
                    applicationUser.ClientId = user.ClientId;
                    applicationUser.PozitieFirma = user.PozitieFirma;
                    applicationUser.Nume = user.Nume;
                    applicationUser.PhoneNumber = user.PhoneNumber;

                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["ClientId"] = new SelectList(_context.Client.ToList(), "ClientId", "Denumire", user.ClientId);
            return View(user);
        }

        private bool UserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
        #endregion

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

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.FirstOrDefault(m => m.Id == id);
            if (applicationUser == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking!" });
            }
            if (applicationUser.LockoutEnd != null && applicationUser.LockoutEnd > DateTime.Now)
            {
                // user is currently locked, we will unlock them
                applicationUser.LockoutEnd = DateTime.Now;
            }
            else
            {
                // user is unlocked, we will lock them
                applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _context.SaveChanges();
            return Json(new { success = true, message = "Operation Successful!" });
        }
        #endregion

    }
}
