using System.ComponentModel.DataAnnotations;

namespace Licenta.Models.Plati
{
    public class TipPlata
    {
        [Key]
        public int TipPlataId { get; set; }
        [Required]
        public string Denumire { get; set; }
    }
}
