using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class ClientFurnizor
    {
        public int ClientFurnizorId { get; set; }
        [Display(Name = "Client")]
        public int ClientId { get; set; }
        // Navigation Property
        public Client Client { get; set; }
        [Display(Name = "Furnizor")]
        public int FurnizorId { get; set; }
        // Navigation Property
        public Furnizor Furnizor { get; set; }
    }
}
