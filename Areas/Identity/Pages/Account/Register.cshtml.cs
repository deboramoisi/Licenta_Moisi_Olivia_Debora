using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Licenta.Data;
using Licenta.Models;
using Licenta.Services.MailService;
using Licenta.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Licenta.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly Licenta.Services.MailService.IEmailSender _emailSender;
        // Role Manager
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            Licenta.Services.MailService.IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Telefon")]
            public string PhoneNumber { get; set; }

            [RegularExpression(@"^[A-Z][a-z]+\s[A-Z][a-z]+$", ErrorMessage = "Numele trebuie sa fie de tip: 'Nume Prenume'. Va rugam reincercati."), Required, StringLength(50, MinimumLength = 3)]
            public string Nume { get; set; }

            [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "Pozitia ocupata in firma trebuie sa fie de minim 3 caractere."), Display(Name = "Pozitie ocupata")]
            public string PozitieFirma { get; set; }

            [Display(Name = "Selectati rolul utilizatorului")]
            public string Rol { get; set; }

            // Exista si utilizatori individuali care nu apartin unei companii
            // Clienti care doresc doar consultatii
            [Display(Name = "Selectati firma apartinatoare")]
            public int? ClientId { get; set; }

            // pentru dropdown clienti
            public IEnumerable<SelectListItem> ClientList { get; set; }
            // pentru dropdown roluri
            public IEnumerable<SelectListItem> RolList { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel()
            {
               // populare lista clienti
               ClientList = _context.Client.ToList().Select(i => new SelectListItem { 
                    Text = i.Denumire,
                    Value = i.ClientId.ToString()
               }),
               // populare lista roluri
               RolList = _roleManager.Roles.Where(u => u.Name != ConstantVar.Rol_User_Individual).Select(x => x.Name).Select(i => new SelectListItem { 
                    Text = i,
                    Value = i
               })
            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    ClientId = Input.ClientId,
                    PhoneNumber = Input.PhoneNumber,
                    PozitieFirma = Input.PozitieFirma,
                    Nume = Input.Nume,
                    Rol = Input.Rol
                };
                // creare user
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Check if the role exists
                    if (!await _roleManager.RoleExistsAsync(ConstantVar.Rol_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(ConstantVar.Rol_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(ConstantVar.Rol_Admin_Firma))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(ConstantVar.Rol_Admin_Firma));
                    }
                    if (!await _roleManager.RoleExistsAsync(ConstantVar.Rol_User_Individual))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(ConstantVar.Rol_User_Individual));
                    }
                    if (!await _roleManager.RoleExistsAsync(ConstantVar.Rol_Angajat))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(ConstantVar.Rol_Angajat));
                    }

                    if (user.Rol == null)
                    {
                        await _userManager.AddToRoleAsync(user, ConstantVar.Rol_User_Individual);    
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, user.Rol);
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    IEnumerable<EmailAddress> emailAddresses = new List<EmailAddress>() {
                        new EmailAddress() {
                            Address = user.Email
                        }
                    };

                    var webRoot = _env.WebRootPath;
                    var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailConfirmation.html";

                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string messageBody = string.Format(builder.HtmlBody,
                        user.Nume,
                        HtmlEncoder.Default.Encode(callbackUrl)
                        );

                    var message = new Message(emailAddresses, "Confirmare cont mail ", messageBody);
                    _emailSender.SendEmail(message);

                    // sign in user after registration
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Rol == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            // admin-ul adauga noi Useri si il redirectionam spre lista cu toti Userii
                            return RedirectToAction("Index", "ApplicationUsers", new { Area = "Admin"});
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
