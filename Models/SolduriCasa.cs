using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Models
{
    public class SolduriCasa
    {
        [Key]
        public int SolduriCasaId { get; set; }

        [Display(Name = "Data")]
        public DateTime data { get; set; }
        
        [Display(Name = "Sold ziua precedenta")]
        public float sold_prec { get; set; }

        [Display(Name = "Incasari")]
        public float incasari { get; set; }

        [Display(Name = "Plati")]
        public float plati { get; set; }

        [Display(Name = "Sold final luna")]
        public float sold_zi { get; set; }

        public Client Client { get; set; }
        [ForeignKey("ClientId"), Display(Name = "Client")]
        public int ClientId { get; set; }
    }
}
