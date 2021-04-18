namespace Licenta.Models.Chat
{
    public class ChatUser
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public UserChatRole Role { get; set; }
    }
}