using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Models.Plati
{
    public class Plata
    {
        [Key]
        public int PlataId { get; set; }
        
        [Required]
        public float Suma { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public DateTime Data { get; set; }

        [Display(Name = "Data scadenta")]
        public DateTime DataScadenta { get; set; }

        [Required, Display(Name = "Tip plata")]
        public int TipPlataId { get; set; }
        [ForeignKey("TipPlataId")]
        public TipPlata TipPlata { get; set; }


        public bool Achitata { get; set; } = false;
        public bool SuccesPlata { get; set; } = false;
    }
}