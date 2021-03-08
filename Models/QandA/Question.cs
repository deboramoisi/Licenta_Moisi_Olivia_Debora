using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models.QandA
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        
        [Required, MinLength(length: 10, ErrorMessage = "Intrebarea adresata trebuie sa aiba minim 10 caractere!" )]
        public string Intrebare { get; set; }
        
        [Display(Name = "Detaliati intrebarea")]
        public string Descriere { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Adaugare")]
        public DateTime DataAdaugare { get; set; } = DateTime.Now;
        
        public bool Rezolvata { get; set; } = false;
        
        [Display(Name = "Categorie")]
        public int QuestionCategoryId { get; set; }
        [ForeignKey("QuestionCategoryId")]
        public QuestionCategory QuestionCategory { get; set; }

        [Display(Name = "Utilizator")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        // Navigation property
        public IEnumerable<Response> Responses { get; set; }
    }
}
