using Licenta.Areas.Admin.Models.ViewModels;
using Licenta.Data;
using Licenta.Services.DashboardManager;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]

    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDashboardManager _dashboardManager;

        public DashboardController(ApplicationDbContext context, IDashboardManager dashboardManager)
        {
            _context = context;
            _dashboardManager = dashboardManager;
        }

        public IActionResult Index()
        {
            DashboardVM dvm = new DashboardVM() { };
            ViewData["ClientIdList"] = new SelectList(_context.Client.OrderBy(c => c.Denumire), "ClientId", "Denumire");
            return View(dvm);
        }

        // API calls - denumire client, profit pierdere pt diagrame
        #region
        [HttpGet]
        public async Task<IActionResult> GetDenumireClient(string id)
        {
            return Json(await _dashboardManager.GetDenumireClient(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetProfitPierdere(string id, string an)
        {
            return Json(await _dashboardManager.GetProfitPierdere(id, an));
        }

        [HttpGet]
        public async Task<IActionResult> GetSolduriCasa(string id, string an)
        {
            return Json(await _dashboardManager.GetSolduriCasa(id, an));
        }
        #endregion
    }
}
