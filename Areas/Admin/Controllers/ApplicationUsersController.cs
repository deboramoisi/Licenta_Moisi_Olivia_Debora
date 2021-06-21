using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public ApplicationUsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers
                                .Include(c => c.Client)
                                .OrderBy(u => u.Nume)
                                .ToListAsync());
        }

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
            var userRole = await _userManager.GetRolesAsync(user);

            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire).ToList(), "ClientId", "Denumire");
            ViewData["Roluri"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            ViewBag.RolId = userRole[0];

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

                    var oldRole = _userManager.GetRolesAsync(applicationUser).Result;
                    await _userManager.RemoveFromRolesAsync(applicationUser, oldRole);
                    await _userManager.AddToRoleAsync(applicationUser, user.Rol);

                    _context.ApplicationUsers.Update(applicationUser);
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
            var userRole = await _userManager.GetRolesAsync(user);

            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire).ToList(), "ClientId", "Denumire", user.ClientId);
            ViewData["Roluri"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name", user.Rol);
            ViewBag.RolId = userRole[0];

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
        public async Task<IActionResult> GetAll()
        {
            var allObj = _context.ApplicationUsers.Include(c => c.Client);
            return Json(new { data = await allObj.ToListAsync() });
        }

        // Id-ul user-ului este de tip string!!!
        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(string id)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea userului!" });
            }
            else
            {
                _context.ApplicationUsers.Remove(applicationUser);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "User sters cu succes!" });
            }
        }

        [HttpGet]
        public string GenerateRandomPassword()
        {
            var options = _userManager.Options.Password;

            int passLength = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digits = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // generate minimum Length password
            while (password.Length < passLength)
            {
                char character = (char)random.Next(32, 126);

                password.Append(character);

                if (char.IsDigit(character))
                    digits = false;
                else if (char.IsLower(character))
                    lowercase = false;
                else if (char.IsUpper(character))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(character))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digits)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
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
