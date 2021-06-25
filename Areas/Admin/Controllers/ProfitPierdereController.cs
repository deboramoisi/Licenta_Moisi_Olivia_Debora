using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.ViewModels;
using Licenta.Services.FileManager;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using Licenta.Areas.Admin.Models.ViewModels;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class ProfitPierdereController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfitPierdereController(ApplicationDbContext context, IFileManager fileManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        // Index, Details
        #region
        public async Task<IActionResult> Index()
        {
            var profitPierdere = _context.ProfitPierdere.Include(d => d.Client).OrderBy(u => u.ClientId);
                //if (pp.rulaj_c > pp.rulaj_d)
                //{
                //    pp.Profit_luna = pp.rulaj_c - pp.rulaj_d;
                //} 
                //else
                //{
                //    pp.Pierdere_luna = pp.rulaj_d - pp.rulaj_c;
                //}
                //_context.Update(pp);
            return View(await profitPierdere.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.ProfitPierdere
                .FirstOrDefaultAsync(m => m.ProfitPierdereId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }
        #endregion

        // Create, import new document
        #region
        [HttpGet]
        public IActionResult Create()
        {
            var balanta = new ImportBalanteXMLVM() { };
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return View(balanta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportBalanteXMLVM balanta)
        {
            var user = await _userManager.GetUserAsync(User);
            var documentType = _context.TipDocument.FirstOrDefault(u => u.Denumire == "XML").TipDocumentId;

            Document document = new Document()
            {
                ApplicationUserId = user.Id,
                ClientId = balanta.ClientId,
                TipDocumentId = documentType,
                DocumentPath = await _fileManager.SaveDocument(balanta.DocumentPathUrl, "XML", balanta.ClientId, user.Id),
            };

            if (ModelState.IsValid)
            {
                _context.Document.Add(document);

                var fullPath = $"C:/Users/user/source/repos/Licenta/wwwroot/xml/{document.DocumentPath}";
                XDocument doc = XDocument.Load(fullPath);

                var balante = from bal in doc.Root.Elements()
                              where bal.Element("cont").Value.ToString().Equals("121")
                              select bal;

                foreach (XElement bal in balante)
                {
                    ProfitPierdere profitPierdere = CreateProfitPierdere(bal, balanta);
                    _context.Add(profitPierdere);
                }

                _fileManager.DeleteDocumentXML(document.DocumentPath);
                _context.Document.Remove(document);
                _context.SaveChanges();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", balanta.ClientId);
            TempData["Message"] = "Solduri profit-pierdere importate cu succes!";
            TempData["Success"] = "true";
            return RedirectToAction(nameof(Index));
        }

        private static ProfitPierdere CreateProfitPierdere(XElement profitP, ImportBalanteXMLVM balanta)
        {
            ProfitPierdere profitPierdere = new ProfitPierdere()
            {
                deb_prec = float.Parse(profitP.Element("deb_prec").Value),
                cred_prec = float.Parse(profitP.Element("cred_prec").Value),
                rulaj_d = float.Parse(profitP.Element("rulaj_d").Value),
                rulaj_c = float.Parse(profitP.Element("rulaj_c").Value),
                fin_d = float.Parse(profitP.Element("fin_d").Value),
                fin_c = float.Parse(profitP.Element("fin_c").Value),
                ClientId = balanta.ClientId,
                Year = balanta.Year,
                Month = balanta.Month
            };

            // daca veniturile sunt mai mari atunci avem profit
            if (profitPierdere.rulaj_c > profitPierdere.rulaj_d)
            {
               
                profitPierdere.Profit_luna = profitPierdere.rulaj_c - profitPierdere.rulaj_d;
            }
            // altfel avem pierdere
            else
            {
                profitPierdere.Pierdere_luna = profitPierdere.rulaj_c - profitPierdere.rulaj_d;
            }

            return profitPierdere;
        }
        #endregion

        // Create Manual
        #region
        public IActionResult ManualCreate()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire), "ClientId", "Denumire");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManualCreate(ProfitPierdere profitPierdere)
        {
            if (ModelState.IsValid)
            {
                _context.ProfitPierdere.Add(profitPierdere);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Solduri profit pierdere adaugate cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire), "ClientId", "Denumire", profitPierdere.ClientId);
            return View(profitPierdere);
        }
        #endregion

        // Edit, edit uploaded document
        #region
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profitPierdere = await _context.ProfitPierdere.FindAsync(id);
            if (profitPierdere == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire), "ClientId", "Denumire", profitPierdere.ClientId);
            return View(profitPierdere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProfitPierdere profitPierdere)
        {
            if (id != profitPierdere.ProfitPierdereId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ProfitPierdere.Update(profitPierdere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfitPierdereExists(profitPierdere.ProfitPierdereId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Solduri profit pierdere actualizate cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire), "ClientId", "Denumire", profitPierdere.ClientId);
            return View(profitPierdere);
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.DocumentId == id);
        }
        #endregion

        // API CALLS
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = _context.ProfitPierdere
                .Include(d => d.Client)
                .OrderBy(d => d.ClientId)
                    .ThenByDescending(d => d.Year)
                    .ThenBy(d => d.Month);

            return Json(new { data = await allObj.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            ProfitPierdere profitPierdere = _context.ProfitPierdere.Find(id);
            if (profitPierdere == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea soldurilor de profit/pierdere!" });
            }
            else
            {
                try
                {
                    _context.ProfitPierdere.Remove(profitPierdere);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Solduri profit/pierdere sterse cu succes!" });
                }
                catch
                {
                    return Json(new { success = false, message = "Eroare la stergerea soldurilor de profit/pierdere!" });
                }
            }
        }
        #endregion

        // Delete solduri profit/pierdere
        #region
        [HttpGet]
        public IActionResult DeleteSolduriBalanta()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return PartialView("_DeleteSolduriBalanta");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSolduriBalanta(DeleteSolduriBalantaVM soldVM)
        {
            var year = soldVM.DataEnd.Year;
            var month = soldVM.DataStart.Month;

            _context.ProfitPierdere.RemoveRange(_context.ProfitPierdere.Where(
                                        x => x.ClientId == soldVM.ClientId 
                                        && (int.Parse(x.Year) >= soldVM.DataStart.Year && int.Parse(x.Year) <= soldVM.DataEnd.Year)
                                        && (int.Parse(x.Month) >= soldVM.DataStart.Month && int.Parse(x.Month) <= soldVM.DataEnd.Month)));
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        private bool ProfitPierdereExists(int id)
        {
            return _context.ProfitPierdere.Any(e => e.ProfitPierdereId == id);
        }
    }
}
