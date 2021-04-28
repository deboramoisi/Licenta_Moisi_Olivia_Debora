using Licenta.Data;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin_Firma)]

    public class InformatiiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformatiiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Furnizori()
        {
            return View();
        }

        public IActionResult Salariati()
        {
            return View();
        }

        public IActionResult Plati()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFurnizori()
        {
            try
            {
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var furnizori = await _context.Furnizori
                    .Where(u => u.ClientId == user.ClientId)
                    .OrderBy(u => u.denumire)
                    .AsNoTracking()
                    .ToListAsync();
                return Json(new { data = furnizori });
            }
            catch
            {
                return Json(new { data = 0 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSalariati()
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var salariati = (user != null) ? (_context.Salariat.Where(u => u.ClientId == user.ClientId).OrderBy(u => u.Nume)) : null;
            return Json(new { data = await salariati.ToListAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> GetPlati()
        {
            var clientId = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).ClientId;
            var plati = await _context.Plati
                .Include(x => x.Client)
                .Include(x => x.TipPlata)
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.Data)
                .AsNoTracking()
                .ToListAsync();

            return Json(new { data = plati });
        }
       
    }
}
