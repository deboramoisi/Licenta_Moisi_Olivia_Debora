using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Licenta.ViewModels;
using Licenta.Services.FileManager;
using System.Xml.Linq;
using Licenta.Areas.Admin.Models.ViewModels;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin + "," + ConstantVar.Rol_Admin_Firma)]

    public class SalariatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public SalariatsController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // Details & Index
        #region
        public async Task<IActionResult> Index()
        {
            return View(await _context.Salariat.Include(c => c.Client).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salariat = await _context.Salariat
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.SalariatId == id);
            if (salariat == null)
            {
                return NotFound();
            }

            return View(salariat);
        }
        #endregion

        // Create, Edit
        #region
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salariat salariat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salariat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View(salariat);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salariat = await _context.Salariat.FindAsync(id);
            if (salariat == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", salariat.ClientId);
            return View(salariat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Salariat salariat)
        {
            if (id != salariat.SalariatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salariat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalariatExists(salariat.SalariatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", salariat.ClientId);
            return View(salariat);
        }
       
        private bool SalariatExists(int id)
        {
            return _context.Salariat.Any(e => e.SalariatId == id);
        }
        #endregion

        // API CALLS: Get all, Delete
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = _context.Salariat.Include(c => c.Client);
            return Json(new { data = await allObj.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            Salariat salariat = _context.Salariat.Find(id);
            if (salariat == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea salariatului!" });
            }
            else
            {
                _context.Remove(salariat);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Salariat sters cu succes!" });
            }
        }
        #endregion

        // Import Saga, Delete Salariati Modal
        #region
        [HttpGet]
        public IActionResult ImportSalariati()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return PartialView("_AddSalariatiImport");
        }

        [HttpPost]
        public async Task<IActionResult> ImportSalariati(int? id)
        {
            DocumentVM documentVM = new DocumentVM() { };
            documentVM.ApplicationUserId = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            var documentTip = await _context.TipDocument.FirstOrDefaultAsync(u => u.Denumire == "Salariati XML");

            if (id != 0)
            {
                documentVM.ClientId = id.Value;
            }

            // preluam documentele primite prin ajax
            var files = Request.Form.Files;

            // parcurgem fiecare document si il adaugam
            foreach (var file in files)
            {
                Document document = new Document()
                {
                    ApplicationUserId = documentVM.ApplicationUserId,
                    ClientId = documentVM.ClientId,
                    TipDocumentId = documentTip.TipDocumentId,
                    DocumentPath = await _fileManager.SaveDocument(file, documentTip.Denumire, documentVM.ClientId, documentVM.ApplicationUserId),
                    Data = DateTime.Now
                };

                if (ModelState.IsValid)
                {
                    _context.Add(document);

                    // procesam XML-ul
                    // adaugam salariatii preluati din acesta clientului ales de utilizator

                    var fullPath = $"C:/Users/user/source/repos/Licenta/wwwroot{document.DocumentPath}";
                    XDocument doc = XDocument.Load(fullPath);

                    var salariati = from salariat in doc.Root.Elements()
                                    select salariat;

                    foreach (XElement salariat in salariati)
                    {
                        Salariat salariatNou = new Salariat
                        {
                            Nume = salariat.Element("nume").Value.ToString(),
                            Prenume = salariat.Element("prenume").Value.ToString(),
                            locatie = salariat.Element("locatie").Value.ToString(),
                            functie = salariat.Element("functie").Value.ToString(),
                            datai = DateTime.Parse(salariat.Element("datai").Value),
                            tip = salariat.Element("tip").Value.ToString(),
                            ore_zi = int.Parse(salariat.Element("ore_zi").Value),
                            grupa = salariat.Element("grupa").Value.ToString(),
                            nr_zile_co = int.Parse(salariat.Element("nr_zile_co").Value),
                            tip_rem = salariat.Element("tip_rem").Value.ToString(),
                            salar_brut = int.Parse(salariat.Element("salar_brut").Value),
                            cn = salariat.Element("cn").Value.ToString(),
                            judet = salariat.Element("judet").Value.ToString(),
                            localitate = salariat.Element("localitate").Value.ToString(),
                            str = salariat.Element("str").Value.ToString(),
                            nr = salariat.Element("nr").Value.ToString(),
                            cod_post = salariat.Element("cod_post").Value.ToString(),
                            nr_contr = int.Parse(salariat.Element("nr_contr").Value),
                            d_contract = DateTime.Parse(salariat.Element("d_contract").Value),
                            ClientId = document.ClientId
                        };

                        _context.Add(salariatNou);
                    }
                    // stergem din memorie: bd si server XML-ul
                    _fileManager.DeleteDocumentXML(document.DocumentPath);
                    _context.Remove(document);
                    _context.SaveChanges();
                }
            }
            return PartialView("_AddSalariatiImport", documentVM);
        }

        public IActionResult DeleteSalariati()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return PartialView("_DeleteSalariatiClient");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSalariati(DeleteFurnizoriVM salariatVM)
        {
            _context.Salariat.RemoveRange(_context.Salariat.Where(x => x.ClientId == salariatVM.ClientId));
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
