using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models.Chat
{
    public class Message
    {
        
        [Key]
        public int MessageId { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Text { get; set; }
        
        public DateTime When { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
