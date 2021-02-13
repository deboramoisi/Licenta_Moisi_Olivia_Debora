using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DateTime DataConcediere { get; set; }
        public IstoricSalar IstoricSalar { get; set; }
    }
}
