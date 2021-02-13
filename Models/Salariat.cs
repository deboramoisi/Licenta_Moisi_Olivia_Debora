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
        
        [Required, DataType(DataType.Date),Display(Name = "Data Angajare")]
        public DateTime DataAngajare { get; set; }
        
        [DataType(DataType.Date), Display(Name = "Data concediere")]
        public DateTime? DataConcediere { get; set; }
        
        // Navigation Property Istoric Salar 
        public ICollection<IstoricSalar> IstoricSalar { get; set; }

        [ForeignKey("Client"), Display(Name = "Client")]
        public int ClientId { get; set; }
        
        // Navigation Property Client
        public Client Client { get; set; }
    }
}
