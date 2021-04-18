using System;

namespace Licenta.Models.Chat
{
    public class Mesaj
    {
        public int MesajId { get; set; }
        public string Nume { get; set; }
        public string Text { get; set; }
        public DateTime Data { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}