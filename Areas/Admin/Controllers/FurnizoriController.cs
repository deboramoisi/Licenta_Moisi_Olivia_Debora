using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.ViewModels;
using Licenta.Services.FileManager;
using System.Xml.Linq;
using Licenta.Areas.Admin.Models.ViewModels;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FurnizoriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public FurnizoriController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // GET: Admin/Furnizoris
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Furnizori.Include(f => f.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnizori = await _context.Furnizori
                .Include(f => f.Client)
                .FirstOrDefaultAsync(m => m.FurnizorID == id);
            if (furnizori == null)
            {
                return NotFound();
            }

            return View(furnizori);
        }

        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "CodCAEN");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FurnizorID,denumire,cod_fiscal,tara,judet,adresa,ClientId")] Furnizori furnizori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(furnizori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "CodCAEN", furnizori.ClientId);
            return View(furnizori);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnizori = await _context.Furnizori.FindAsync(id);
            if (furnizori == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "CodCAEN", furnizori.ClientId);
            return View(furnizori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FurnizorID,denumire,cod_fiscal,tara,judet,adresa,ClientId")] Furnizori furnizori)
        {
            if (id != furnizori.FurnizorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(furnizori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurnizoriExists(furnizori.FurnizorID))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "CodCAEN", furnizori.ClientId);
            return View(furnizori);
        }

        private bool FurnizoriExists(int id)
        {
            return _context.Furnizori.Any(e => e.FurnizorID == id);
        }

        [HttpGet]
        public IActionResult ImportFurnizori()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return PartialView("_AddFurnizoriImport");
        }

        [HttpPost]
        public async Task<IActionResult> ImportFurnizori(int? id) {
            DocumentVM documentVM = new DocumentVM() { };
            documentVM.ApplicationUserId = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            var documentTip = await _context.TipDocument.FirstOrDefaultAsync(u => u.Denumire == "Furnizori XML");

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
                    _context.SaveChanges();
                    
                    // procesam XML-ul
                    // adaugam furnizorii preluati din acesta clientului ales de utilizator

                    var fullPath = $"C:/Users/user/source/repos/Licenta/wwwroot{document.DocumentPath}";
                    XDocument doc = new XDocument();
                    doc = XDocument.Load(fullPath);

                    var furnizori = from furnizor in doc.Root.Elements()
                                    select furnizor;

                    foreach (XElement furnizor in furnizori)
                    {
                        Furnizori furnizorNou = new Furnizori
                        {
                            denumire = furnizor.Element("denumire").Value.ToString(),
                            cod_fiscal = furnizor.Element("cod_fiscal").Value.ToString(),
                            tara = furnizor.Element("tara").Value.ToString(),
                            judet = furnizor.Element("judet").Value.ToString(),
                            adresa = furnizor.Element("adresa").Value.ToString(),
                            ClientId = document.ClientId
                        };

                        _context.Add(furnizorNou);
                    }
                    // stergem din memorie: bd si server XML-ul
                    _fileManager.DeleteDocumentXML(document.DocumentPath);
                    _context.Remove(document);
                    _context.SaveChanges();
                }
            }
            return PartialView("_AddFurnizoriImport", documentVM);
        }

        public IActionResult DeleteFurnizori()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.OrderBy(u => u.Denumire), "ClientId", "Denumire");
            return PartialView("_DeleteFurnizoriClient");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFurnizori(DeleteFurnizoriVM furnizoriVM)
        {
            _context.Furnizori.RemoveRange(_context.Furnizori.Where(x => x.ClientId == furnizoriVM.ClientId));
            await _context.SaveChangesAsync();
            return Ok();
        }

        // API CALLS
        #region
        public IActionResult GetAllFurnizori()
        {
            var furnizori = _context.Furnizori.OrderBy(u => u.ClientId).ToList();
            return Json(new { data = furnizori });
        }
        #endregion


    }
}
