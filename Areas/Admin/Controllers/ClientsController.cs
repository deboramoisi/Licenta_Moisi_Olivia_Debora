using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Licenta.Models;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;
using System.Xml.Linq;
using Licenta.Services.FileManager;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public ClientsController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // Index, Details
        #region
        public async Task<IActionResult> Index()
        {
            return View(await _context.Client
                                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .Include(b => b.Salariati)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }
        #endregion

        // Edit, Create
        #region
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {

            if (ModelState.IsValid)
            {
                _context.Add(client);
                _context.SaveChanges();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ClientId == id);

            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(client);
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.ClientId == id);
        }
        #endregion

        // API CALLS
        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = _context.Client;
            return Json(new { data = await allObj.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            Client client = _context.Client.Find(id);
            if (client == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea clientului!" });
            }
            else
            {
                _context.Remove(client);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Client sters cu succes!" });
            }
        }
        #endregion

        // Import Clienti from Saga XML
        #region

        [HttpGet]
        public IActionResult ImportClienti()
        {
            return PartialView("_AddClientiImport");
        }

        [HttpPost, ActionName("ImportClienti")]
        public async Task<IActionResult> ImportClientiPost()
        {
            DocumentVM documentVM = new DocumentVM() { };
            documentVM.ApplicationUserId = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            var documentTip = await _context.TipDocument.FirstOrDefaultAsync(u => u.Denumire == "Clienti XML");

            // preluam documentele primite prin ajax
            var files = Request.Form.Files;

            // parcurgem fiecare document si il adaugam
            foreach (var file in files)
            {
                Document document = new Document()
                {
                    ApplicationUserId = documentVM.ApplicationUserId,
                    ClientId = 1,
                    TipDocumentId = documentTip.TipDocumentId,
                    DocumentPath = await _fileManager.SaveDocument(file, documentTip.Denumire, 1, documentVM.ApplicationUserId),
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
                    doc = XDocument.Load(document.DocumentPath);

                    var clienti = from client in doc.Root.Elements()
                                    select client;

                    foreach (XElement client in clienti)
                    {
                        Client clientNou = new Client
                        {
                           Denumire = client.Element("nume").Value.ToString(),
                           CodFiscal = int.Parse(client.Element("cod_fiscal").Value.ToString()),
                           NrRegComertului = client.Element("reg_com").Value.ToString()
                        };

                        _context.Add(clientNou);
                    }
                    // stergem din memorie: bd si server XML-ul
                    _fileManager.DeleteDocumentXML(document.DocumentPath);
                    _context.Remove(document);
                    _context.SaveChanges();
                }
            }

            return PartialView("_AddClientiImport");
        }
        #endregion
    }
}
