using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public ICollection<SolduriCasa> SolduriCasa { get; set; }
        public ICollection<Salariat> Salariati { get; set; }
        public ICollection<Furnizori> Furnizori { get; set; }
        public ICollection<ProfitPierdere> ProfitPierdere { get; set; }

    }
}
