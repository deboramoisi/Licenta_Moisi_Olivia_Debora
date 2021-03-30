using Licenta.Models.QandA;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Areas.Clienti.Models
{
    public class QaVM
    {
        [Key]
        public int ResponseId { get; set; }

        [Required, MinLength(length: 10, ErrorMessage = "Raspunsul acordat trebuie sa aiba minim 10 caractere!")]
        public string Raspuns { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Adaugare")]
        public DateTime DataAdaugare { get; set; } = DateTime.Now;

        [Display(Name = "Intrebare"), Required]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [Display(Name = "Categorie"), Required]
        public int QuestionCategoryId { get; set; }
        [ForeignKey("QuestionCategoryId")]
        public QuestionCategory QuestionCategory { get; set; }
    }
}
