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

    public class SalariatiController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SalariatiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // API CALLS
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var salariati = (user != null) ? (_context.Salariat.Where(u => u.ClientId == user.ClientId).OrderBy(u => u.Nume)) : null;
            return Json(new { data = await salariati.ToListAsync() }); 
        }
        #endregion
    }
}
