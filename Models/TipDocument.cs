using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class TipDocument
    {
        [Key]
        public int TipDocumentId { get; set; }
        
        [Required]
        [MinLength(length: 2, ErrorMessage = "Denumirea documentului trebuie sa fie de minim 2 caractere!")]
        public string Denumire { get; set; }
    }
}
