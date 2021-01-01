using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class Furnizor
    {
        public int FurnizorID { get; set; }

        [Required, MinLength(5, ErrorMessage = "Denumirea furnizorului trebuie sa fie cel putin 5 caractere")]
        public string Denumire { get; set; }
        // Navigation Property Client m:n (ClientiFurnizori n:1)
        public ICollection<ClientFurnizor> ClientFurnizori { get; set; }
    }
}
