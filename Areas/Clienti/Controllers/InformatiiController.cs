using Licenta.Data;
using Licenta.Models.Notificari;
using Licenta.Models.Plati;
using Licenta.Services.MailService;
using Licenta.Services.NotificationManager;
using Licenta.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender; 

        public InformatiiController(ApplicationDbContext context,
            INotificationManager notificationManager,
            IWebHostEnvironment env,
            IEmailSender emailSender)
        {
            _context = context;
            _notificationManager = notificationManager;
            _env = env;
            _emailSender = emailSender;
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
            var webRoot = _env.WebRootPath;
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var customerService = new CustomerService();
            Customer customer = customerService.Get(session.CustomerId);

            Plata plata = await _context.Plati
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.PlataId == plata_id);

            plata.Achitata = true;
            plata.SuccesPlata = true;
            _context.Plati.Update(plata);
            await _context.SaveChangesAsync();

            // send notification of payment for admin
            Notificare notificare = new Notificare()
            {
                Text = $"{User.Identity.Name} a achitat plata pentru serviciile pentru luna {plata.Data}",
                RedirectToPage = "/Admin/Plata"
            };
            await _notificationManager.CreateAsyncNotificationForAdmin(notificare, _context.ApplicationUsers.First(x => x.Email.Contains("dana_moisi")).Id);
            // notificare redirectToPage

            // send mail with invoice
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "PaymentConfirmation.html";

            var builder = new BodyBuilder();
            using(StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                User.Identity.Name,
                plata.Suma,
                plata.Data
                );

            IEnumerable<EmailAddress> emailAddresses = new List<EmailAddress>() {
                        new EmailAddress() {
                            Address = User.Identity.Name
                        }
                    };

            var message = new Message(emailAddresses, "Plata cu succes servicii Contsal", messageBody);
            _emailSender.SendEmail(message);

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
                    SuccessUrl = "https://localhost:44342" + "/order/success?session_id={CHECKOUT_SESSION_ID}&plata_id=" + id,
                    CancelUrl = "https://localhost:44342" + "/order/declined?session_id={CHECKOUT_SESSION_ID}&plata_id=" + id,
            };
            var service = new SessionService();
            // create transaction on the credit/debit card
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }

    }

}
