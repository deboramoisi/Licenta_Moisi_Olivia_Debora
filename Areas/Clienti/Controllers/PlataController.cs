using Licenta.Data;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Authorize(Roles = ConstantVar.Rol_Admin_Firma)]
    [Area("Clienti")]

    public class PlataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientId = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).ClientId;
            var plati = await _context.Plati
                .Include(x => x.Client)
                .Include(x => x.TipPlata)
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.Data)
                .AsNoTracking()
                .ToListAsync();

            return Json(new { data = plati});
        }
    }
}
