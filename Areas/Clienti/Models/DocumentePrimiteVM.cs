using System;

namespace Licenta.Areas.Clienti.Models
{
    public class DocumentePrimiteVM
    {
        public int DocumentId { get; set; }
        public int TipDocumentId { get; set; }
        public string TipDocument { get; set; }
        public DateTime Data { get; set; }
        public string DocumentPath { get; set; }
    }
}
