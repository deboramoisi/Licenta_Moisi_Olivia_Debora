using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class Salariat
    {
        public int SalariatId { get; set; }
        
        [Required]
        public string Nume { get; set; }
        
        [Required]
        public string Prenume { get; set; }
        
        [Required]
        public string Pozitie { get; set; }
        
        [Required, DataType(DataType.Date), Display(Name = "Data Angajare"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DataAngajare { get; set; }
        
        [DataType(DataType.Date), Display(Name = "Data concediere"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DataConcediere { get; set; }
        
        [Display(Name = "Istoric Salar")]
        // Navigation Property Istoric Salar 
        public ICollection<IstoricSalar> IstoricSalar { get; set; }

        [ForeignKey("Client"), Display(Name = "Client")]
        public int ClientId { get; set; }

        // Navigation Property Client
        public Client Client { get; set; }

        // Nume complet pentru select list sau diferite afisari
        [NotMapped]
        [Display(Name = "Nume Prenume")]
        public string NumePrenume
        {
            get
            {
                return Nume + " " + Prenume;
            }
        }
    }
}
