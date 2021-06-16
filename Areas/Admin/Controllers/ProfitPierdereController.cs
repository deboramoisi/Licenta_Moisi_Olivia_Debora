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

                var fullPath = $"C:/Users/user/source/repos/Licenta/wwwroot{document.DocumentPath}";
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

        // Edit, edit uploaded document
        #region
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.ProfitPierdere.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire), "ClientId", "Denumire", document.ClientId);
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentVM documentVM)
        {
            var document = await _context.Document.FindAsync(documentVM.DocumentId);

            if (id != document.DocumentId)
            {
                return NotFound();
            }

            if (documentVM.DocumentPathUrl != null)
            {
                var documentType = _context.TipDocument.Find(documentVM.TipDocumentId);
                document.DocumentPath = await _fileManager.UpdateDocument(documentVM.DocumentPathUrl, documentVM.DocumentId, documentType.Denumire, documentVM.ClientId, documentVM.ApplicationUserId);
            } 

            if (ModelState.IsValid)
            {
                try
                {
                    document.ApplicationUserId = documentVM.ApplicationUserId;
                    document.ClientId = documentVM.ClientId;
                    document.TipDocumentId = documentVM.TipDocumentId;
                    document.Data = documentVM.Data;
                    _context.Document.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Solduri profit-pierdere actualizate cu succes!";
                TempData["Success"] = "true";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(x => x.Denumire), "ClientId", "Denumire", document.ClientId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(x => x.Nume), "Id", "Nume", document.ApplicationUserId);
            ViewData["TipDocumentId"] = new SelectList(_context.TipDocument.OrderBy(x => x.Denumire), "TipDocumentId", "Denumire", document.TipDocumentId);
            return View(documentVM);
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
            Document document = _context.Document.Find(id);
            if (document == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea documentului!" });
            }
            else
            {
                _context.Document.Remove(document);
                await _context.SaveChangesAsync();
                if (_fileManager.DeleteDocument(document.DocumentPath) == "Success")
                {
                    return Json(new { success = true, message = "Document sters cu succes!" });
                } else
                {
                    return Json(new { success = false, message = "Eroare la stergerea documentului!" });
                }
            }
        }

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
        #endregion
    }
}
