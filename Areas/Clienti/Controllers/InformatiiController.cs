using Licenta.Data;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
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

        public InformatiiController(ApplicationDbContext context)
        {
            _context = context;
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

        public IActionResult SuccesPlata()
        {
            return View();
        }

        public IActionResult EsecPlata()
        {
            return View();
        }

        [Route("/order/success")]
        [HttpGet]
        public ActionResult OrderSuccess([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var customerService = new CustomerService();
            Customer customer = customerService.Get(session.CustomerId);

            return Content($"<html><body><h1>Multumim pentru plata realizata, {customer.Name}!</h1></body></html>");
        }

        [HttpPost]
        public IActionResult Charge(string stripeEmail, string stripeToken)
        {
            var clienti = new CustomerService();
            var plati = new ChargeService();

            var domain = "http://localhost:5001/Clienti/Informatii";
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
                      UnitAmount = 2000,
                      Currency = "ron",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Stubborn Attachments",
                      },
                    },
                    Quantity = 1,
                  },
                },
                    Mode = "payment",
                    SuccessUrl = "http://localhost:5001" + "/order/success?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = domain + "/EsecPlata",
                };
                var service = new SessionService();
                Session session = service.Create(options);

                return Json(new { id = session.Id });
            }
        }

}
