using Licenta.Models;
using Licenta.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.ChatManager
{
    public interface IChatManager
    {
        Task<bool> CreateRoom(string name, string admin);
        Task<int> CreatePrivateRoom(string userId, string admin);
        Task<bool> JoinRoom(string connectionId, string roomId);
        Task<bool> JoinRoom(int id, string userId);
        Task<bool> LeaveRoom(string connectionId, string roomId);
        Task<int> CreateMessage(int chatId, string message, string user);
        Task<bool> SendMessage(int roomId, string message, string user);
        IList<ApplicationUser> Find(string userId);
        IList<Chat> Private(string userId);

    }
}
