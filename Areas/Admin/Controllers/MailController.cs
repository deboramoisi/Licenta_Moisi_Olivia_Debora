using Microsoft.AspNetCore.Mvc;
using Licenta.Services.MailService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Licenta.Utility;
using Licenta.Data;
using System.Linq;

namespace Licenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    
    public class MailController : Controller
    {
        private readonly IEmailSender _mailSender;
        private readonly ApplicationDbContext _context; 

        public MailController(ApplicationDbContext context,
            IEmailSender mailSender)
        {
            _context = context;
            _mailSender = mailSender;
        }

        [HttpGet]
        public IActionResult SendMail()
        {
            IEnumerable<EmailAddress> emailAddresses = new List<EmailAddress>() {
               new EmailAddress() { 
                    Address = "deboramoisi@yahoo.com"
               }
            }; 
     
            var message = new Message(emailAddresses, "Test2", "This is our message for the test");

            try
            {
                _mailSender.SendEmail(message);
                return Json(new { success = true, message = "Mail sent successfully!"});
            }
            catch
            {
                return Json(new { success = false, message = "Error!" });
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
