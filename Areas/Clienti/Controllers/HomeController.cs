using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Licenta.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Licenta.Data;
using Licenta.Models.Chat;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Licenta.Areas.Clienti.Models;
using Licenta.Services.MailService;
using System.Collections.Generic;
using MimeKit;

namespace Licenta.Areas.Clienti.Controllers
{
    [Area("Clienti")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(Licenta.Models.Chat.Message message)
        {
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.UserId = sender.Id;
                // adaugam mesajul cu toate informatiile
                await _context.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Error();
        }

        public async Task<IActionResult> Chat()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var messages = await _context.Messages.OrderBy(m => m.When).ToListAsync();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
                return View(messages);
            }
            else
            {
                // Pentru return spre Login
                return LocalRedirect("/Identity/Account/Login");
            }

        }

        [HttpPost]
        public IActionResult SendForm(HomeFormVM form)
        {
            IEnumerable<EmailAddress> emails = new List<EmailAddress>() { 
                new EmailAddress{ Address = "deboramoisi@yahoo.com", DisplayName = "Debi" }
            };

            if (ModelState.IsValid)
            {
                var message = new Licenta.Services.MailService.Message(emails, form.Subiect, form.Mesaj + " Email: " + form.Email);
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
