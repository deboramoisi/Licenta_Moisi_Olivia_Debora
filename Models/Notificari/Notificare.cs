using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Licenta.Models.Notificari
{
    public class Notificare
    {
        [Key]
        public int NotificareId { get; set; }
        [Required]
        public string Text { get; set; }
        public List<NotificareUser> NotificareUsers { get; set; }
    }
}
