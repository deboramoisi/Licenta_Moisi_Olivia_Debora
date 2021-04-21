using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Models.Notificari
{
    public class NotificareUser
    {
        public int NotificareId { get; set; }
        [ForeignKey("NotificareId")]
        public Notificare Notificare { get; set; }
        
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        
        public bool Seen { get; set; } = false;
    }
}
