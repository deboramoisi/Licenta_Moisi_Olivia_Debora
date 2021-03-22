using Licenta.Areas.Clienti.Models;
using Licenta.Data;
using Licenta.Services.DashboardManager;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin_Firma)]
    public class DashboardClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDashboardManager _dashboardManager;

        public DashboardClientController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IDashboardManager dashboardManager)
        {
            _context = context;
            _userManager = userManager;
            _dashboardManager = dashboardManager;
        }

        public IActionResult Index()
        {
            DashboardVM dvm = new DashboardVM() { };
            return View(dvm);
        }

        // API calls - denumire client, profit pierdere pt diagrame
        #region
        [HttpGet]
        public async Task<IActionResult> GetDenumireClient()
        {
            var clientId = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name).ClientId;
            return Json(await _dashboardManager.GetDenumireClient(clientId.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> GetProfitPierdere(string an)
        {
            var clientId = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name).ClientId;
            return Json(await _dashboardManager.GetProfitPierdere(clientId.ToString(), an));
        }
        #endregion


    }
}
