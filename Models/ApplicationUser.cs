using Licenta.Models.Chat;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    // Add new columns to AspNetUsers
    public class ApplicationUser : IdentityUser
    {
        [RegularExpression(@"^[A-Z][a-z]+\s[A-Z][a-z]+$", ErrorMessage = "Numele trebuie sa fie de tip: 'Nume Prenume'. Va rugam reincercati."), Required, StringLength(50, MinimumLength = 3)]
        public string Nume { get; set; }
        
        [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "Pozitia ocupata in firma trebuie sa fie de minim 3 caractere.")]
        public string PozitieFirma { get; set; }

        [NotMapped]
        public string Rol { get; set; }

        // Exista si utilizatori individuali care nu apartin unei companii
        // Clienti care doresc doar consultatii
        public int? ClientId { get; set; }

        // Navigation Property
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public string Descriere { get; set; }
        public string ProfileImageUrl { get; set; }

        // 1 - * ApplicationUser || Message
        public ICollection<Message> Messages { get; set; }

        // un user poate avea mai multe chats (incluzand grupurile)
        public ICollection<ChatUser> Chats { get; set; }


        public ApplicationUser()
        {
            Messages = new HashSet<Message>();
        }
    }
}
