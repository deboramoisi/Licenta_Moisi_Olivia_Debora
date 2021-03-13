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
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Client
                                .Include(c => c.SediuSocial)
                                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                // Includem detalii din Sediu Social
                .Include(b => b.SediuSocial)
                .Include(b => b.Salariati)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

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
                .Include(b => b.SediuSocial)
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
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Denumire,NrRegComertului,CodCAEN,TipFirma,CapitalSocial,CasaDeMarcat,TVA")] Client client)
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

        // API CALLS
        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _context.Client.Include(c => c.SediuSocial).ToList();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult DeleteAPI(int id)
        {
            Client client = _context.Client.Find(id);
            if (client == null)
            {
                return Json(new { success = false, message = "Eroare la stergerea clientului!" });
            }
            else
            {
                _context.Remove(client);
                _context.SaveChanges();
                return Json(new { success = true, message = "Client sters cu succes!" });
            }
        }
        #endregion

    }
}
