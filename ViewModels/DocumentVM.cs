using Licenta.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels
{
    public class DocumentVM
    {
        [Key]
        public int DocumentId { get; set; }

        public IFormFile DocumentPathUrl { get; set; }

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
    }
}
