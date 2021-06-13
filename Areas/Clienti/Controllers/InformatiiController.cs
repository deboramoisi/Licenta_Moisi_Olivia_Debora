using Licenta.Data;
using Licenta.Models.Notificari;
using Licenta.Models.Plati;
using Licenta.Services.NotificationManager;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    [Authorize(Roles = ConstantVar.Rol_Admin_Firma)]

    public class InformatiiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationManager _notificationManager;

        public InformatiiController(ApplicationDbContext context,
            INotificationManager notificationManager)
        {
            _context = context;
            _notificationManager = notificationManager;
        }

        public IActionResult Furnizori()
        {
            return View();
        }

        public IActionResult Salariati()
        {
            return View();
        }

        public IActionResult Plati()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFurnizori()
        {
            try
            {
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var furnizori = await _context.Furnizori
                    .Where(u => u.ClientId == user.ClientId)
                    .OrderBy(u => u.denumire)
                    .AsNoTracking()
                    .ToListAsync();
                return Json(new { data = furnizori });
            }
            catch
            {
                return Json(new { data = 0 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSalariati()
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var salariati = (user != null) ? (_context.Salariat.Where(u => u.ClientId == user.ClientId).OrderBy(u => u.Nume)) : null;
            return Json(new { data = await salariati.ToListAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> GetPlati()
        {
            var clientId = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).ClientId;
            var plati = await _context.Plati
                .Include(x => x.Client)
                .Include(x => x.TipPlata)
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.Data)
                .AsNoTracking()
                .ToListAsync();

            return Json(new { data = plati });
        }


        [Route("/order/declined")]
        [HttpGet]
        public IActionResult OrderDeclined()
        {
            return View();
        }

        [Route("/order/success")]
        [HttpGet]
        public async Task<ActionResult> OrderSuccess([FromQuery] string session_id, [FromQuery] int plata_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var customerService = new CustomerService();
            Customer customer = customerService.Get(session.CustomerId);

            Plata plata = await _context.Plati
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.PlataId == plata_id);

            plata.Achitata = true;
            _context.Plati.Update(plata);
            await _context.SaveChangesAsync();

            // send notification of payment for admin
            Notificare notificare = new Notificare()
            {
                Text = $"{User.Identity.Name} a achitat plata pentru serviciile pentru luna {plata.Data}"
            };
            await _notificationManager.CreateAsyncNotificationForAdmin(notificare, _context.ApplicationUsers.First(x => x.Email.Contains("dana_moisi")).Id);
            // notificare redirectToPage

            // send mail with invoice

            return View(plata);
        }

        [HttpPost]
        public IActionResult Charge(int id)
        {
            Plata plata = _context.Plati.FirstOrDefault(x => x.PlataId == id);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                    LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (int)Math.Round(plata.Suma * 100),
                      Currency = "ron",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Plata Servicii Contsal",
                      },
                    },
                    Quantity = 1,
                  },
                },
                    Mode = "payment",
                    SuccessUrl = "https://localhost:5001" + "/order/success?session_id={CHECKOUT_SESSION_ID}&plata_id=" + id,
                    CancelUrl = "https://localhost:5001" + "/order/declined?session_id={CHECKOUT_SESSION_ID}&plata_id=" + id,
            };
            var service = new SessionService();
            // create transaction on the credit/debit card
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }

    }

}
