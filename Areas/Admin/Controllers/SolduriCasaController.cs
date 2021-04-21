using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Licenta.Services.FileManager;
using Licenta.ViewModels;
using System.Xml.Linq;
using Licenta.Areas.Admin.Models.ViewModels;

namespace Licenta.Areas.Admin.Views
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]

    public class SolduriCasaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public SolduriCasaController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // Details, Index
        #region
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SolduriCasa.Include(s => s.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var solduriCasa = await _context.SolduriCasa
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.SolduriCasaId == id);
            if (solduriCasa == null)
            {
                return NotFound();
            }

            return View(solduriCasa);
        }
        #endregion

        // Create and Edit
        #region
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolduriCasa solduriCasa)
        {
            if (ModelState.IsValid)
            {
                _context.SolduriCasa.Add(solduriCasa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", solduriCasa.ClientId);
            return View(solduriCasa);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var solduriCasa = await _context.SolduriCasa.FindAsync(id);
            if (solduriCasa == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", solduriCasa.ClientId);
            return View(solduriCasa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SolduriCasa solduriCasa)
        {
            if (id != solduriCasa.SolduriCasaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.SolduriCasa.Update(solduriCasa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolduriCasaExists(solduriCasa.SolduriCasaId))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "Denumire", solduriCasa.ClientId);
            return View(solduriCasa);
        }

        private bool SolduriCasaExists(int id)
        {
            return _context.SolduriCasa.Any(e => e.SolduriCasaId == id);
        }
        #endregion

        // API CALLS: get all, delete
        #region
        [HttpGet]
        public async Task<IActionResult> GetAllSolduri()
        {
            var solduri = _context.SolduriCasa.Include(u => u.Client).OrderBy(u => u.ClientId).ThenByDescending(u => u.data);
            return Json(new { data = await solduri.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var sold = _context.SolduriCasa.FirstOrDefault(u => u.SolduriCasaId == id);

            if (id == 0 || sold == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea soldului!" });
            } 
            else
            {
                _context.Remove(sold);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Sold sters cu succes!" });
            }

        }
        #endregion

        // Import solduri
        #region
        public IActionResult ImportSolduri()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire).ToList(), "ClientId", "Denumire");
            return PartialView("_AddSolduriImport");
        }

        [HttpPost]
        public async Task<IActionResult> ImportSolduri(int? id)
        {
            DocumentVM documentVM = new DocumentVM() { };
            documentVM.ApplicationUserId = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            var documentTip = await _context.TipDocument.FirstOrDefaultAsync(u => u.Denumire == "XML");

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
                    _context.Document.Add(document);

                    // procesam XML-ul
                    // adaugam salariatii preluati din acesta clientului ales de utilizator

                    var fullPath = $"C:/Users/user/source/repos/Licenta/wwwroot{document.DocumentPath}";
                    XDocument doc = XDocument.Load(fullPath);

                    var solduri = from sold in doc.Root.Elements()
                                  where DateTime.Parse(sold.Element("data").Value).Day > 25 
                                  select sold;

                    IList<SolduriCasa> solduriCasa = new List<SolduriCasa>();
                    
                    // Adaugam doar soldurile care sunt dupa data de 25 a lunii
                    foreach (XElement sold in solduri)
                    {
                        SolduriCasa soldNou = CreateSold(sold, document.ClientId);
                        solduriCasa.Add(soldNou);
                    }
                    
                    // pastram soldurile din ultima data cu activitate a lunii 
                    for (var i = 0; i < solduriCasa.Count - 1; i++)
                    {
                        if (solduriCasa[i].data.Month.ToString() == solduriCasa[i+1].data.Month.ToString()
                            && solduriCasa[i].data.Day < solduriCasa[i+1].data.Day)
                        {
                            solduriCasa.Remove(solduriCasa[i]);
                            i--;
                        }
                    }                   
                   
                    // adaugam cate un sold pentru fiecare luna
                    foreach (SolduriCasa sold in solduriCasa)
                    {
                        _context.SolduriCasa.Add(sold);
                    }
                    
                    // stergem din memorie: bd si server XML-ul
                    _fileManager.DeleteDocumentXML(document.DocumentPath);
                    _context.Document.Remove(document);
                    _context.SaveChanges();
                }
            }
            return PartialView("_AddSolduriImport", documentVM);
        }

        private static SolduriCasa CreateSold(XElement sold, int clientId)
        {
            SolduriCasa soldNou = new SolduriCasa
            {
                data = DateTime.Parse(sold.Element("data").Value),
                sold_prec = float.Parse(sold.Element("sold_prec").Value),
                incasari = float.Parse(sold.Element("incasari").Value.ToString()),
                plati = float.Parse(sold.Element("plati").Value.ToString()),
                sold_zi = float.Parse(sold.Element("sold_zi").Value),
                ClientId = clientId
            };
            return soldNou;
        }
        #endregion

        // Delete solduri
        #region
        public IActionResult DeleteSolduri()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return PartialView("_DeleteSolduriClient");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSolduri(DeleteFurnizoriVM soldVM)
        {
            _context.SolduriCasa.RemoveRange(_context.SolduriCasa.Where(x => x.ClientId == soldVM.ClientId));
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
