using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Licenta.Data;
using Microsoft.AspNetCore.Identity;
using Licenta.Models;
using Licenta.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Licenta.Models.CereriDocumente;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Licenta.Services.NotificationManager;
using Licenta.Models.Notificari;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin + "," + ConstantVar.Rol_Admin_Firma)]

    public class CereriDocumenteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationManager _notificationManager;

        public CereriDocumenteController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            INotificationManager notificationManager)
        {
            _context = context;
            _userManager = userManager;
            _notificationManager = notificationManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var salariati = _context.Salariat
                .Where(x => x.ClientId == user.ClientId)
                .ToList();

            CerereDocument cerere = new CerereDocument();

            if (await _userManager.IsInRoleAsync(user, ConstantVar.Rol_Admin_Firma))
            {
                cerere.ApplicationUserId = user.Id;
                cerere.DenumireClient = _context.Client.FindAsync(user.ClientId.Value).Result.Denumire;
                ViewData["Salariati"] = new SelectList(salariati, "SalariatId", "NumePrenume");
            }
            else
            {
                ViewData["Salariati"] = new SelectList(_context.Salariat.ToList(), "SalariatId", "NumePrenume");
            }

            cerere.TipCerere = _context.TipCereri.ToList();
            ViewData["TipCereri"] = new SelectList(cerere.TipCerere, "TipCerereId", "Denumire");

            return View(cerere);
        }

        // API CALLS
        #region
        [HttpPost]
        public async Task<IActionResult> Create(CerereDocument data)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var admin = _context.ApplicationUsers.Where(x => x.Email.Contains("dana_moisi")).FirstOrDefault();

            if (ModelState.IsValid)
            {
                data.DenumireCerere = _context.TipCereri.Find(data.TipCerereId).Denumire;
                var salariat = _context.Salariat.FirstOrDefault(x => x.SalariatId == data.SalariatId);
                string redirectToPage = "/Clienti/CereriDocumente/Index";

                if (data.CerereDocumentId != 0)
                {
                    // edit
                    _context.CereriDocumente.Update(data);
                    await _context.SaveChangesAsync();
                    // trimitem notificari adminilor
                    Notificare notificare = new Notificare();
                    notificare.Text = user.Nume + " a editat cererea de " + data.DenumireCerere + " pentru firma " + data.DenumireClient + ", salariatul "
                                        + salariat.NumePrenume + " deadline la " + data.DataStart;
                    notificare.RedirectToPage = redirectToPage;
                    await _notificationManager.CreateAsyncNotificationForAdmin(notificare, admin.Id);

                    return Json(new { success = true, message = "Cerere editata cu succes!" });
                } else
                {
                    // add
                    _context.CereriDocumente.Add(data);
                    await _context.SaveChangesAsync();
                    // trimitem notificari adminilor
                    Notificare notificare = new Notificare();
                    notificare.Text = user.Nume + " a depus o cerere de " + data.DenumireCerere + " pentru firma " + data.DenumireClient + ", salariatul "
                                        + salariat.NumePrenume + " deadline la " + data.DataStart;
                    notificare.RedirectToPage = redirectToPage;
                    await _notificationManager.CreateAsyncNotificationForAdmin(notificare, admin.Id);
                    
                    return Json(new { success = true, message = "Cerere adaugata cu succes!" });
                }
            }
            return Json(new { success = false, message = "A aparut o eroare, va rugam reincercati!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetCereriCalendar()
        {
            var user = await _userManager.GetUserAsync(User);
            var cereriDocumente = new List<CerereDocument>();
            if (_userManager.GetRolesAsync(user).Result.Contains(ConstantVar.Rol_Admin))
            {
                cereriDocumente = await CereriDocumenteForAdmin();
            } else
            {
                cereriDocumente = await CereriDocumenteForClienti();
            }
            return Json(new { cereri = cereriDocumente, success = true });
        }

        [HttpGet]
        public async Task<IActionResult> CerereDocumentById(int id)
        {

            if (id == 0)
            {
                return Json(new { success = false, message = "Va rugam selectati o cerere!" });
            }

            var cerere = await _context.CereriDocumente
                .Include(x => x.ApplicationUser)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Salariati)
                .Include(x => x.TipCerere)
                .Where(x => x.CerereDocumentId == id)
                .SingleOrDefaultAsync();

            if (cerere != null)
            {
                return Json(new { cerere = cerere, success = true });
            } else
            {
                return Json(new { success = false, message = "A aparut o eroare!" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCerere(int id)
        {
            if (id == 0)
            {
                return Json(new { success = false, message = "Va rugam selectati o cerere!" });
            }

            var cerere = await _context.CereriDocumente.FirstOrDefaultAsync(x => x.CerereDocumentId == id);
            if (cerere == null)
            {
                return Json(new { success = false, message = "Cererea nu a putut fi gasita!" });
            } else
            {
                _context.CereriDocumente.Remove(cerere);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cerere stearsa cu succes!" });
            }
        }
        #endregion

        public async Task<List<CerereDocument>> CereriDocumenteForAdmin()
        {
            return await _context.CereriDocumente
                .Include(x => x.ApplicationUser)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Salariati)
                .Include(x => x.TipCerere)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<CerereDocument>> CereriDocumenteForClienti()
        {
            var user = _userManager.GetUserAsync(User).Result;

            var client = _context.Client.Find(user.ClientId);

            return await _context.CereriDocumente.Where(x => x.DenumireClient == client.Denumire)
                .Include(x => x.ApplicationUser)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Salariati)
                .Include(x => x.TipCerere)
                .AsNoTracking()
                .ToListAsync();
        }
 
    }
    
}
