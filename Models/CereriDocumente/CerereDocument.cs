using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models.CereriDocumente
{
    public class CerereDocument
    {
        public int CerereDocumentId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }

        public IEnumerable<TipCerere> TipCerere { get; set; }
        [ForeignKey("TipCerereId")]
        public int TipCerereId { get; set; }

        public string DenumireClient { get; set; }
        public string DenumireCerere { get; set; }
        public string DataStart { get; set; }

        public IEnumerable<Salariat> Salariati { get; set; }
        [ForeignKey("SalariatId")]
        public int SalariatId { get; set; }

        public bool Resolved { get; set; } = false;
        public string AdeverintaPath { get; set; }

        [NotMapped]
        [Display(Name = "Incarcati documentul dumneavoastra")]
        public IFormFile DocumentPathUrl { get; set; }
    }
}
