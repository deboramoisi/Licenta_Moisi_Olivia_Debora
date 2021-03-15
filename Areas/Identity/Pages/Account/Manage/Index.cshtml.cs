using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Data;
using Licenta.Models;
using Licenta.Services.FileManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IFileManager fileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _fileManager = fileManager;
        }

        public string Username { get; set; }
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string profileImageUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
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
            [Display(Name = "Companie")]
            public string ClientName { get; set; } = "";
            public string Descriere { get; set; }

            [BindProperty]
            public IFormFile ImageUrl { get; set; }

            public string ProfileImageUrl { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var applicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);

            Username = userName;
            Email = applicationUser.Email;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ClientId = applicationUser.ClientId,
                ClientName = (applicationUser.ClientId != 0) ? (_context.Client.Find(applicationUser.ClientId).Denumire) : "",
                PozitieFirma = applicationUser.PozitieFirma,
                Nume = applicationUser.Nume,
                Rol = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                Descriere = applicationUser.Descriere,
                ProfileImageUrl = applicationUser.ProfileImageUrl
            };

            profileImageUrl = $"/img/profile/{Input.ProfileImageUrl}";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var applicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == user.Id);

            if (Input.ImageUrl != null)
            {
                // user-ul vrea sa editeze fotografia de profil
                applicationUser.ProfileImageUrl = await _fileManager.SaveImage(Input.ImageUrl);
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (applicationUser != null)
            {
                if (applicationUser.Nume != Input.Nume)
                {
                    applicationUser.Nume = Input.Nume;
                }
                if (applicationUser.PozitieFirma != Input.PozitieFirma)
                {
                    applicationUser.PozitieFirma = Input.PozitieFirma;
                }
                if (applicationUser.Descriere != Input.Descriere)
                {
                    applicationUser.Descriere = Input.Descriere;
                }
                _context.Update(applicationUser);
                await _context.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        //public IActionResult OnGetUpdateImage()
        //{
        //    return Page();
        //}

        public async Task<IActionResult> OnPostUpdateImage(string Username, IFormFile ImageUrl)
        {
            var applicationUser = _context.ApplicationUsers.Find(Username);
            if (ImageUrl != null)
            {
                // user-ul vrea sa editeze fotografia de profil
                applicationUser.ProfileImageUrl = await _fileManager.SaveImage(ImageUrl);
            }
            _context.Update(applicationUser);
            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(applicationUser);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

    }
}
