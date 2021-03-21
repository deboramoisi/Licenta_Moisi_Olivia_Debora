using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required, Display(Name = "Cod fiscal")]
        public int CodFiscal { get; set; }

        // Validare Nr Reg Comert: J|C|F[1-59]/[1-600000]/zi.luna.an
        [Display(Name = "Nr. Registrul Comertului"), Required]
        [RegularExpression(@"^[JFC][1-5][0-9]\/[0-9]{1,6}\/(0[1-9]|1\d|2\d|3[01])\.(0[1-9]|1[0-2])\.(19|20)\d{2}$", ErrorMessage = "Numarul nu este valid! Va rugam reincercati.")]
        public string NrRegComertului { get; set; }

        [Display(Name = "Cod CAEN")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Codul CAEN nu este valid! Va rugam reincercati.")]
        public string CodCAEN { get; set; }

        [Display(Name = "Tip firma")]
        public string TipFirma { get; set; } = "Persoana juridica";

        [Display(Name = "Capital Social")]
        public double CapitalSocial { get; set; } = 200;

        [Display(Name = "Casa de marcat")]
        public string CasaDeMarcat { get; set; }

        public string TVA { get; set; }


        public ICollection<SolduriCasa> SolduriCasa { get; set; }
        public ICollection<Salariat> Salariati { get; set; }
        public ICollection<Furnizori> Furnizori { get; set; }
        public ICollection<ProfitPierdere> ProfitPierdere { get; set; }

    }
}
