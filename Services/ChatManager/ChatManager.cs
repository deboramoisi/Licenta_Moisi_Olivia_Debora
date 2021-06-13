using Licenta.Data;
using Licenta.Hubs;
using Licenta.Models;
using Licenta.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.ChatManager
{
    public class ChatManager : IChatManager
    {

        private IHubContext<ChatHub> _chat;
        private readonly ApplicationDbContext _context;

        public ChatManager(
            IHubContext<ChatHub> chat,
            ApplicationDbContext context)
        {
            _chat = chat;
            _context = context;
        }

        public async Task<bool> CreateRoom(string name, string admin)
        {
            var chat = new Chat
            {
                Nume = name,
                Tip = TipChat.Grup
            };

            chat.Users.Add(new ChatUser
            {
                // find user
                ApplicationUserId = admin,
                Role = UserChatRole.Admin
            });

            try
            {
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> CreatePrivateRoom(string userId, string admin) {
            var chat = new Chat
            {
                Tip = TipChat.Privat
            };

            chat.Users.Add(new ChatUser
            {
                ApplicationUserId = userId
            });

            chat.Users.Add(new ChatUser
            {
                ApplicationUserId = admin 
            });

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return chat.ChatId;
        }

        // Pentru conectare la acelasi connectionId - signalR
        public async Task<bool> JoinRoom(string connectionId, string roomId)
        {
            try
            {
                await _chat.Groups.AddToGroupAsync(connectionId, roomId);
                return true;
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<bool> LeaveRoom(string connectionId, string roomId)
        {
            try
            {
                await _chat.Groups.RemoveFromGroupAsync(connectionId, roomId);
                return true;
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<bool> JoinRoom(int id, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                ApplicationUserId = userId,
                Role = UserChatRole.Membru
            };

            try
            {
                _context.ChatUsers.Add(chatUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> CreateMessage(int chatId, string message, string user)
        {
            var Mesajul = new Mesaj
            {
                ChatId = chatId,
                Text = message,
                Nume = user,
                Data = DateTime.Now
            };

            _context.Mesaje.Add(Mesajul);
            await _context.SaveChangesAsync();

            return Mesajul.MesajId;
        }

        public async Task<bool> SendMessage(
            int roomId,
            string message,
            string user)
        {

            int mesajId = await CreateMessage(roomId, message, user);

            try
            {
                if (mesajId != 0)
                {
                    await _chat.Clients.Group(roomId.ToString())
                        // ReceiveMessage will exist on the client, when we execute this line
                        // we will send the message to the client and will execute this declared method
                        .SendAsync("ReceiveMessage", new
                        {
                            Text = message,
                            Nume = user,
                            Data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                        });

                    return true;
                } 
                return false;
            }
            catch
            {
                throw;
            }
        }

        public IList<ApplicationUser> Find(string userId)
        {
            var users = _context.ApplicationUsers
                .Where(x => x.Id != userId)
                .ToList();

            return users;
        }

        public IList<Chat> Private(string userId)
        {
            var chats = _context.Chats
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.Tip == TipChat.Privat
                    && x.Users
                        .Any(y => y.ApplicationUserId == userId))
                .ToList();

            return chats;
        }

    }
}