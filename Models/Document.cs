using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Display(Name = "Document")]
        public string DocumentPath { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Display(Name = "Utilizator")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Tip Document")]
        public int TipDocumentId { get; set; }
        [ForeignKey("TipDocumentId")]
        public TipDocument TipDocument { get; set; }

        [DataType(DataType.Date), Display(Name = "Data document")]
        public DateTime Data { get; set; }
    }
}
