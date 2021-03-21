using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class ProfitPierdere
    {
        [Key]
        public int ProfitPierdereId { get; set; }
        
        [Display(Name = "Balanta XML")]
        public string DocumentPath { get; set; }

        [Display(Name = "Client"), Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public float? deb_prec { get; set; }
        public float? cred_prec { get; set; }

        [Display(Name = "Cheltuieli luna")]
        public float? rulaj_d { get; set; }

        [Display(Name = "Venituri luna")]
        public float? rulaj_c { get; set; }

        [Display(Name = "Pierdere luna")]
        public float? fin_d { get; set; }

        [Display(Name = "Profit luna")]
        public float? fin_c { get; set; }

        [Display(Name = "Profit luna curenta")]
        public float? Profit_luna { get; set; }

        [Display(Name = "Pierdere luna curenta")]
        public float? Pierdere_luna { get; set; }

        public string Year { get; set; }
        public string Month { get; set; }
    }
}
