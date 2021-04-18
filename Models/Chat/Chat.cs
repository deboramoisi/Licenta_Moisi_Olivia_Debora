using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models.Chat
{
    public class Chat
    {
        public Chat()
        {
            Mesaje = new List<Mesaj>();
            Users = new List<ChatUser>();
        }

        [Key]
        public int ChatId { get; set; }
        public ICollection<Mesaj> Mesaje { get; set; }
        public ICollection<ChatUser> Users { get; set; }
        public string Nume { get; set; }
        public TipChat Tip { get; set; }
    }
}
