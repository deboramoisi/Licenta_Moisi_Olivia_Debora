using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required, MinLength(5, ErrorMessage = "Denumirea clientului trebuie sa fie minim 5 caractere")]
        public string Denumire { get; set; }

        [Display(Name = "Nr. Registrul Comertului"), Required]
        public string NrRegComertului { get; set; }

        [Display(Name = "Cod CAEN")]
        public string CodCAEN { get; set; }

        [Display(Name = "Tip firma")]
        public string TipFirma { get; set; } = "Persoana juridica";

        [Display(Name = "Capital Social"), Required]
        public double CapitalSocial { get; set; } = 150;

        [Display(Name = "Casa de marcat")]
        public string CasaDeMarcat { get; set; }

        [Required]
        public string TVA { get; set; }


        // Navigation Property Sediu Social 1:1
        public SediuSocial SediuSocial { get; set; }

        [Display(Name = "Furnizori")]
        // Navigation Property Furnizor m:n (ClientiFurnizori n:1)
        public ICollection<ClientFurnizor> ClientFurnizori { get; set; }
    }
}
