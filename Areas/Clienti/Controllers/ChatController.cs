using Licenta.Areas.Clienti.Models;
using Licenta.Data;
using Licenta.Hubs;
using Licenta.Models;
using Licenta.Models.Chat;
using Licenta.Services.ChatManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        private IChatManager _chatManager;
        private IHubContext<ChatHub> _chat;
        private readonly ApplicationDbContext _context;

        public ChatController(
            IChatManager chatManager,
            IHubContext<ChatHub> chat,
            ApplicationDbContext context
            )
        {
            _chatManager = chatManager;
            _chat = chat;
            _context = context;
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

            if (await _chatManager.SendMessage(roomId, message, User.Identity.Name)) {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("[action]")]
        public IActionResult Find()
        {
            IList<ApplicationUser> users = _chatManager.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(users);
        }

        [HttpGet("[action]")]
        public IActionResult Private()
        {
            IList<Chat> chats = _chatManager.Private(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(chats);
        }

        [HttpPost("[action]/{userId}")]
        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            int chatId = await _chatManager.CreatePrivateRoom(userId, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (chatId != 0)
            {
                return RedirectToAction("Chat", new { id = chatId });
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost("[action]/{name}")]
        public async Task<IActionResult> CreateRoom(string name)
        {

            if (await _chatManager.CreateRoom(name, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpGet("[action]")]
        public IActionResult CreateRoomModal()
        {
            return PartialView("_AddChatRoom", new Chat());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRoomModal(Chat chat)
        {
            chat.Tip = TipChat.Grup;

            chat.Users.Add(new ChatUser
            {
                // find user
                ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserChatRole.Admin
            });

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return PartialView("_AddChatRoom", chat);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> JoinRoom(int id)
        {
            if (await _chatManager.JoinRoom(id, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return RedirectToAction("Chat", new { id = id });
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

        [HttpGet("[action]/{id}")]
        public IActionResult Chat(int id)
        {
            var chat = _context.Chats
                .Include(x => x.Mesaje)
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefault(x => x.ChatId == id);

            var allChats = _context.Chats
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.Users
                        .Any(y => y.ApplicationUserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();

            var chatVM = new ChatVM
            {
                Chat = chat,
                AllChats = allChats
            };

            return View(chatVM);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            var chats = await _context.Chats
                    .Include(x => x.Users)
                        .ThenInclude(x => x.ApplicationUser)
                    .Where(x => !x.Users.Any(y => y.ApplicationUserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    .ToListAsync();

            return View(chats);
        }
    }
}
