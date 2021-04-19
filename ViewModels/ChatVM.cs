using Licenta.Models.Chat;
using System.Collections.Generic;

namespace Licenta.ViewModels
{
    public class ChatVM
    {
        public Chat Chat { get; set; }
        public IEnumerable<Chat> AllChats { get; set; }
    }
}
