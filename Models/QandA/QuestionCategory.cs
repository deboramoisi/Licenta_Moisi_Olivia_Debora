using System;
using System.ComponentModel.DataAnnotations;

namespace Licenta.Models.QandA
{
    public class QuestionCategory
    {
        [Key]
        public int QuestionCategoryId { get; set; }
        public string Denumire { get; set; }
    }
}