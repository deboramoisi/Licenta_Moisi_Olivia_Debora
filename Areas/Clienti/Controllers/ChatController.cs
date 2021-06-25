using Licenta.Data;
using Licenta.Hubs;
using Licenta.Models;
using Licenta.Models.Chat;
using Licenta.Models.Notificari;
using Licenta.Services.ChatManager;
using Licenta.Services.NotificationManager;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Authorize]
    [Area("Clienti")]
    [Route("[controller]")]

    public class ChatController : Controller
    {
        private readonly IChatManager _chatManager;
        private readonly IHubContext<ChatHub> _chat;
        private readonly ApplicationDbContext _context;
        private readonly INotificationManager _notificationManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(
            IChatManager chatManager,
            IHubContext<ChatHub> chat,
            ApplicationDbContext context,
            INotificationManager notificationManager,
            UserManager<ApplicationUser> userManager)
        {
            _chatManager = chatManager;
            _chat = chat;
            _context = context;
            _notificationManager = notificationManager;
            _userManager = userManager;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            if (await _chatManager.JoinRoom(connectionId, roomId)) {
                return Ok();
            }
            return NotFound();
            
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        {
            if (await _chatManager.LeaveRoom(connectionId, roomId)) {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int roomId,
            string message)
        {
            var chatUsers = await _context.ChatUsers
                .Include(x=>x.ApplicationUser)
                .Where(x => x.ChatId == roomId)
                .AsNoTracking()
                .ToListAsync();

            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            ApplicationUser admin = _context.ApplicationUsers.Where(x => x.Email.Contains("dana_moisi")).FirstOrDefault();

            string sender = "";
            string receiver = "";
            string redirectToPage = "";

            if (user.Email.Contains("dana_moisi"))
            {
                sender = admin.Nume;
                receiver = chatUsers.Where(x => !x.ApplicationUserId.Equals(admin.Id)).First().ApplicationUserId;
                redirectToPage = $"/Chat/Chat";
            } else
            {
                sender = user.Nume;
                receiver = admin.Id;
                redirectToPage = $"/Chat/Chat/{roomId}";
            }

            if (await _chatManager.SendMessage(roomId, message, user.UserName)) {
                Notificare notificare = new Notificare() { };
                notificare.Text = $"{sender} v-a trimis un mesaj - ${DateTime.Now}";
                notificare.RedirectToPage = redirectToPage;
                await _notificationManager.CreateChatNotificationAsync(notificare, receiver);
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> JoinRoom(int id)
        {
            if (await _chatManager.JoinRoom(id, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return RedirectToAction("Chat", new { id });
            }
            return NotFound();
        }

        [HttpPost("[action]/{chatId}/{message}")]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            int mesajId = await _chatManager.CreateMessage(chatId, message, User.Identity.Name);
            if (mesajId != 0)
            {
                return RedirectToAction("Chat", new { id = chatId });
            }
            return NotFound();
        }

        [HttpGet("[action]")]
        public IActionResult Chat(int? id)
        {
            var user = _context.ApplicationUsers
                .Include(x => x.Chats)
                    .ThenInclude(x => x.Chat)
                .FirstOrDefault(x => x.UserName == User.Identity.Name);

            int chatId = 0;

            if (_context.ChatUsers.FirstOrDefault(x => x.ApplicationUserId == user.Id) == null)
            {
                var admin = _context.ApplicationUsers.FirstOrDefault(x => x.UserName.Contains("dana_moisi")).Id;
                chatId = _chatManager.CreatePrivateRoom(user.Id, admin).Result;
            } else
            {
                chatId = _context.ChatUsers.FirstOrDefault(x => x.ApplicationUserId == user.Id).ChatId;
            }

            var chat = new Chat();
            if (id == null)
            {
                chat = _context.Chats
                    .Include(x => x.Mesaje)
                    .Include(x => x.Users)
                        .ThenInclude(x => x.ApplicationUser)
                    .FirstOrDefault(x => x.ChatId == chatId);
            } else
            {
                chat = _context.Chats
                    .Include(x => x.Mesaje)
                    .Include(x => x.Users)
                        .ThenInclude(x => x.ApplicationUser)
                    .FirstOrDefault(x => x.ChatId == id);
            }

            var grupuri = _context.Chats
               .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.Users.Any(y => y.ApplicationUserId == user.Id) && x.Tip.Equals(TipChat.Grup))
                .ToList();

            var privat = _context.Chats
            .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.Users.Any(y => y.ApplicationUserId == user.Id) && x.Tip.Equals(TipChat.Privat))
                .ToList();

        ChatVM chatVM = new ChatVM
            {
                Chat = chat,
                Private = privat, 
                Grupuri = grupuri
            };

            return View(chatVM);
        }

        // See group users and delete them from group
        [HttpGet("[action]")]
        public async Task<IActionResult> GetGroupUsers(int id)
        {
            Chat chat = await _context.Chats
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.ChatId == id)
                .FirstOrDefaultAsync();

            if (chat == null)
            {
                return NotFound();
            }
            else
            {
                return PartialView("_GroupUsers", chat);
            }
        }
    }
}
