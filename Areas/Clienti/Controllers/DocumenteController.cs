using Licenta.Data;
using Licenta.Models;
using Licenta.Utility;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin + "," + ConstantVar.Rol_Admin_Firma)]
    public class DocumenteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumenteController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var viewDocumentVM = new ViewDocumentVM()
            {
                TipDocumente = _context.TipDocument.ToList()
                .Select((item, index) => new SelectListItem
                {
                    Value = item.TipDocumentId.ToString(),
                    Text = item.Denumire
                })
            };
            
            return View(viewDocumentVM);   
        }

        [HttpPost]
        public async Task<IActionResult> TakeForm(ViewDocumentVM viewDocumentVM)
        {
            var documents = new List<Document>();

            var user_identity = await _userManager.GetUserAsync(User);
            ApplicationUser appUser = await _context.ApplicationUsers.FindAsync(user_identity.Id);

            if (user_identity == null || appUser == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(appUser, ConstantVar.Rol_Admin))
            {

                // daca e logat un admin, acesta poate vedea toate documentele din anul, luna, tipul selectat
                documents = _context.Document.Include(d => d.Client).Include(d => d.ApplicationUser).Include(d => d.TipDocument)
                    .Where(d => d.Data.Year.ToString().Equals(viewDocumentVM.Year)
                            && d.Data.Month.ToString().Equals(viewDocumentVM.Month)
                            && d.TipDocumentId == viewDocumentVM.TipulDoc)
                    .ToList();
            }
            else if (!await _userManager.IsInRoleAsync(appUser, ConstantVar.Rol_User_Individual))
            {
                // se cauta documentele clientului, respectand anul ales, luna si tipul documentului de catre utilizator
                documents = _context.Document.Include(d => d.Client).Include(d => d.TipDocument)
                    .Where(d => d.ClientId == appUser.ClientId 
                            && d.Data.Year.ToString().Equals(viewDocumentVM.Year)
                            && d.Data.Month.ToString().Equals(viewDocumentVM.Month)
                            && d.TipDocumentId == viewDocumentVM.TipulDoc)
                    .ToList();
            }

            // Daca nu exista documentul, atunci vom returna o pagina de notificare a erorii catre utilizator
            // urmeaza implementarea acestei pagini
            if (documents.Count == 0)
            {
                return View("DocumentNotFound");
            }

            foreach (var document in documents)
            {
                var an = document.DocumentPath.IndexOf("2021");
                var an_scris = document.DocumentPath.Substring(an, 4);
            }

            ViewData["TipDoc"] = _context.TipDocument.Find(viewDocumentVM.TipulDoc).Denumire;
            return View(documents);
        }

        [HttpGet]
        public async Task<IActionResult> Balanta()
        {
            var documents = new List<Document>();

            var user_identity = await _userManager.GetUserAsync(User);
            ApplicationUser appUser = await _context.ApplicationUsers.FindAsync(user_identity.Id);
           
            if (user_identity == null || appUser == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(appUser, ConstantVar.Rol_Admin))
            {
                documents = _context.Document.Include(d => d.TipDocument).ToList();
            } else if (!await _userManager.IsInRoleAsync(appUser, ConstantVar.Rol_User_Individual))
            {
                documents = _context.Document.Include(d => d.TipDocument).Where(d => d.ClientId == appUser.ClientId).ToList();
            }
            
            foreach(var document in documents)
            {
                var an = document.DocumentPath.IndexOf("2021");
                var an_scris = document.DocumentPath.Substring(an,4);
            }

            return View(documents);
        }

    }
}
