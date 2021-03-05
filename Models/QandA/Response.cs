using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models.QandA
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }
        
        [Required, MinLength(length: 10, ErrorMessage = "Raspunsul acordat trebuie sa aiba minim 10 caractere!")]
        public string Raspuns { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Adaugare")]
        public DateTime DataAdaugare { get; set; } = DateTime.Now;

        [Display(Name = "Intrebare")]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}
