using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class SediuSocial
    {
        [ForeignKey("Client"), Display(Name = "Client")]
        public int SediuSocialId { get; set; }

        [Required]
        public string Localitate { get; set; }
        [Required]
        public string Judet { get; set; }

        public int? Sector { get; set; }
        [Required]
        public string Strada { get; set; }

        // Numar e String pentru ca poate fi si 23B
        public string Numar { get; set; }

        [MaxLength(6), Display(Name = "Cod Postal")]
        public string CodPostal { get; set; }

        public string Bl { get; set; } = "";

        public int? Sc { get; set; }

        public int? Et { get; set; }

        public int? Ap { get; set; }

        // validator pentru numar de telefon
        [RegularExpression(@"^[0][2|3|7][0-9]{8}$", ErrorMessage = "Numarul de telefon este invalid!")]
        [MaxLength(10), Required]
        public string Telefon { get; set; }

        // validator email
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email-ul introdus nu este valid! Va rugam reincercati."), Required]
        public string Email { get; set; }

        // Navigation Property
        public Client Client { get; set; }
    }
}
