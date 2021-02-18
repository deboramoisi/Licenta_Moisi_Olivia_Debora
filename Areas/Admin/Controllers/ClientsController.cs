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
                // Includem Sediu Social pentru a putea afisa detaliile din acel model relatia 1:1 cu clienti
                                .Include(c => c.SediuSocial)
                                .Include(c => c.ClientFurnizori)
                                .ThenInclude(c => c.Furnizor)
                                .ToListAsync());
        }

        // GET: Clients/Details/5
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
                .Include(b => b.ClientFurnizori)
                                .ThenInclude(b => b.Furnizor)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientVM clientVM)
        {
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire");

            Client client = clientVM.Client;

            if (ModelState.IsValid)
            {
                _context.Add(client);
                _context.SaveChanges();
                foreach (var furnizor in clientVM.SelectedFurnizors)
                {
                    if (furnizor != 0)
                    {
                        _context.Add(new ClientFurnizor()
                        {
                            ClientId = client.ClientId,
                            FurnizorId = furnizor
                        });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientVM);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .Include(b => b.SediuSocial)
                .Include(b => b.ClientFurnizori)
                    .ThenInclude(b => b.Furnizor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ClientId == id);

            if (client == null)
            {
                return NotFound();
            }

            var clientFurnizori = _context.ClientFurnizor;
            List<int> selectedFurnizors = new List<int>();

            foreach (var item in clientFurnizori)
            {
                if (item.ClientId == client.ClientId)
                {
                    selectedFurnizors.Add(item.FurnizorId);
                }
            }

            ClientVM clientVM = new ClientVM()
            {
                Client = client,
                SelectedFurnizors = selectedFurnizors
            };

            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire");
            return View(clientVM);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Denumire,NrRegComertului,CodCAEN,TipFirma,CapitalSocial,CasaDeMarcat,TVA,ClientFurnizori")] Client client)
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
            ViewData["FurnizorId"] = new SelectList(_context.Furnizor, "FurnizorID", "Denumire");
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Client.FindAsync(id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            var allObj = _context.Client.Include(c => c.SediuSocial).Include(c => c.ClientFurnizori).ThenInclude(c => c.Furnizor).ToList();
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
