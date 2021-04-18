using Licenta.Models;
using Licenta.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Models
{
    public class ChatVM
    {
        public Chat Chat { get; set; }
        public IEnumerable<Chat> AllChats { get; set; }
    }
}
