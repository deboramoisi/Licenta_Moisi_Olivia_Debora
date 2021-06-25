using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Licenta.Models;
using Licenta.Services.MailService;
using System.Collections.Generic;
using MimeKit;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Licenta.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, 
            IEmailSender sender,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _emailSender = sender;
            _env = env;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString();

                var builder = new BodyBuilder();

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile + "EmailConfirmation.html"))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                string messageBody = string.Format(builder.HtmlBody,
                    user.Nume,
                    callbackUrl
                    );

                IEnumerable<EmailAddress> emailAddresses = new List<EmailAddress>() {
                        new EmailAddress() {
                            Address = Email
                        }
                    };

                var message = new Message(emailAddresses, "Confirmare cont mail ", messageBody);
                _emailSender.SendEmail(message);
            }

            return Page();
        }
    }
}
