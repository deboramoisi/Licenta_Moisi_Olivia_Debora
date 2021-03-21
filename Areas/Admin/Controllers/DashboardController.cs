using Licenta.Data;
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
    [Authorize(Roles = ConstantVar.Rol_Admin + "," + ConstantVar.Rol_Admin_Firma)]

    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ClientIdList"] = new SelectList(_context.Client.OrderBy(c => c.Denumire), "ClientId", "Denumire");
            return View();
        }

        // API calls - denumire client, profit pierdere pt diagrame
        #region
        [HttpGet]
        public async Task<IActionResult> GetDenumireClient(string id)
        {
            var client = await _context.Client.FindAsync(int.Parse(id));
            return Json(client.Denumire);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfitPierdere(string id, string an)
        {
            var profitPierdere = await _context.ProfitPierdere.Where(u => u.ClientId == int.Parse(id) && u.Year == an).ToListAsync();
            IList<float> pp = new List<float>();

            foreach (var pr in profitPierdere)
            {
                if (pr.Profit_luna == null)
                {
                    pp.Add(pr.Pierdere_luna.Value);
                } 
                else
                {
                    pp.Add(pr.Profit_luna.Value);
                }
            }

            return Json(pp.ToArray());
        }
        #endregion
    }
}
