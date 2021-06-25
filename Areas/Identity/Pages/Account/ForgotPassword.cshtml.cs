using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Licenta.Models;
using Licenta.Services.MailService;
using System.IO;
using MimeKit;
using Microsoft.AspNetCore.Hosting;

namespace Licenta.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                IEnumerable<EmailAddress> emailAddresses = new List<EmailAddress>() {
                        new EmailAddress() {
                            Address = user.Email
                        }
                    };

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString();

                    var builder = new BodyBuilder();

                    // Confirmation Mail
                    var codeMail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    codeMail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codeMail));
                    var callbackUrlMail = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = codeMail },
                        protocol: Request.Scheme);

                    builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile + "EmailConfirmation.html"))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string messageBody = string.Format(builder.HtmlBody,
                        user.Nume,
                        HtmlEncoder.Default.Encode(callbackUrlMail)
                        );

                    var messageMail = new Message(emailAddresses, "Confirmare cont mail ", messageBody);
                    _emailSender.SendEmail(messageMail);
                    // Don't reveal that the user does not exist or is not confirmed
                    // return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var message = new Message(emailAddresses, "Resetare parola", 
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                _emailSender.SendEmail(message);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
