using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class Furnizori
    {
        [Key]
        public int FurnizorID { get; set; }

        [Required, MinLength(5, ErrorMessage = "Denumirea furnizorului trebuie sa fie cel putin 5 caractere"), Display(Name = "Denumire")]
        public string denumire { get; set; }
        [Required, Display(Name = "Cod fiscal")]
        public string cod_fiscal { get; set; }

        public Client Client { get; set; }
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
    }
}
