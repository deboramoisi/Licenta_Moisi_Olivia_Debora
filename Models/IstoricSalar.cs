using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class IstoricSalar
    {
        [Display(Name = "Istoric Salar")]
        public int IstoricSalarId { get; set; }

        [ForeignKey("Salariat"), Display(Name = "Salariat")]
        public int SalariatId { get; set; }
        
        [Required, DataType(DataType.Date), Display(Name = "Data incepere contract")]
        public DateTime DataInceput { get; set; }
        
        [DataType(DataType.Date), Display(Name = "Data sfarsit contract")]
        public DateTime? DataSfarsit { get; set; }
        
        [Required]
        public float Salariu { get; set; }
        
        // Navigation Property Salariat
        public Salariat Salariat { get; set; }
    }
}
