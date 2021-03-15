using Licenta.Data;
using Licenta.Models;
using Licenta.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    public class FurnizoriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FurnizoriController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllFurnizori()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(ConstantVar.Rol_Admin) || User.IsInRole(ConstantVar.Rol_Admin_Firma))
                {
                    var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    var furnizori = await _context.Furnizori.Where(u => u.ClientId == user.ClientId).OrderBy(u => u.denumire).ToListAsync();
                    return Json(new { data = furnizori });
                }
            }
            return Json(new { data = 0 });
        }
    }
}
