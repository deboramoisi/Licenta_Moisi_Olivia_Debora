using System.ComponentModel.DataAnnotations;

namespace Licenta.Models.CereriDocumente
{
    public class TipCerere
    {
        [Key]
        public int TipCerereId { get; set; }
        public string Denumire { get; set; }
    }
}
