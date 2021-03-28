using System.ComponentModel.DataAnnotations;

namespace Licenta.Areas.Clienti.Models
{
    public class HomeFormVM
    {
        [Required]
        public string Nume { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Subiect { get; set; }

        [Required]
        public string Mesaj { get; set; }
    }
}
