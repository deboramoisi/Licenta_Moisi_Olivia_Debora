using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Licenta.Models;
using Microsoft.AspNetCore.Identity;
using Licenta.Data;
using Licenta.Areas.Clienti.Models;
using Licenta.Services.MailService;
using System.Collections.Generic;
using System.IO;
using MimeKit;
using Microsoft.AspNetCore.Hosting;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;


        public HomeController(IEmailSender emailSender,
            IWebHostEnvironment env)
        {
            _emailSender = emailSender;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendForm(HomeFormVM form)
        {
            // formare mail
            if (ModelState.IsValid)
            {
                var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailHomeForm.html";

                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                string messageBody = string.Format(builder.HtmlBody,
                    form.Subiect,
                    form.Mesaj,
                    form.Nume,
                    form.Email
                    );

                IEnumerable<EmailAddress> emailAddresses = new List<EmailAddress>() {
                    new EmailAddress{ Address = "deboramoisi@yahoo.com", DisplayName = "Contsal SRL - Debora Moisi" }
                };

                var message = new Message(emailAddresses, form.Subiect, messageBody);

                try
                {
                    _emailSender.SendEmail(message);
                    return Json(new { success = true, message = "Mail-ul dumneavoastra a fost transmis cu succes!"});
                }
                catch
                {
                    throw;
                }

            }
            return Json(new { success = false, message = "Eroare la trimiterea mail-ului!" });

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
