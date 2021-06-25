using Licenta.Data;
using Licenta.Models;
using Licenta.Models.Chat;
using Licenta.Services.ChatManager;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Licenta.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantVar.Rol_Admin)]
    [Area("Admin")]
    [Route("[controller]")]

    public class ChatController : Controller
    {
        private IChatManager _chatManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ChatController(
            IChatManager chatManager,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _chatManager = chatManager;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        {
            if (await _chatManager.LeaveRoom(connectionId, roomId)) {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("[action]/{userId}")]
        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            int chatId = await _chatManager.CreatePrivateRoom(userId, user.Id);
            if (chatId != 0)
            {
                return RedirectToAction("Chat", new { id = chatId });
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> JoinRoom(int id)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            if (await _chatManager.JoinRoom(id, user.Id))
            {
                return RedirectToAction("Chat", new { id = id });
            }
            return NotFound();
        }

        [HttpPost("[action]/{chatId}/{message}")]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            int mesajId = await _chatManager.CreateMessage(chatId, message, user.UserName);
            if (mesajId != 0)
            {
                return RedirectToAction("Chat", new { id = chatId });
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Chat(int id)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            var chat = _context.Chats
                .Include(x => x.Mesaje)
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefault(x => x.ChatId == id);

            IList<Chat> utilizatoriPrivati = _chatManager.Private(user.Id);

            var grupuri = _context.Chats
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.Users.Any(y => y.ApplicationUserId == user.Id) && x.Tip.Equals(TipChat.Grup))
                .ToList();

            var chatVM = new ChatVM
            {
                Chat = chat,
                Private = utilizatoriPrivati,
                Grupuri = grupuri
            };

            return View(chatVM);
        }

        // Create group modal; delete group
        #region
        [HttpGet("[action]")]
        public IActionResult CreateGroup()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(u => u.Nume).ToList(), "Id", "Nume");
            return PartialView("_AddChatRoom");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroupAsync(ChatRoomVM chatRoomVM)
        {
            if (ModelState.IsValid)
            {
                Chat chat = await _chatManager.CreateRoom(chatRoomVM.Name);

                bool addUsersToGroup = await AddUsersToGroup(chatRoomVM.Users, chat);
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(u => u.Nume).ToList(), "Id", "Nume");
            return PartialView("_AddChatRoom");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AddUsersToGroupModal(int? id)
        {
            ICollection<ApplicationUser> notAddedUsers = new List<ApplicationUser>();

            if (id != 0) {
                Chat chat = await _context.Chats.FirstOrDefaultAsync(x => x.ChatId == id);
                notAddedUsers = GetNotAddedUsersToGroup(chat);
            }

            ViewData["ApplicationUserId"] = new SelectList(notAddedUsers.OrderBy(u => u.Nume).ToList(), "Id", "Nume");
            return PartialView("_AddUsersToGroup");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddUsersToGroupModal(int? id, ChatRoomVM chatVM)
        {
            ICollection<ApplicationUser> notAddedUsers = new List<ApplicationUser>();
            if (id != 0)
            {
                Chat chat = await _context.Chats.FirstOrDefaultAsync(x => x.ChatId == id);
                if (chat == null)
                {
                    return NotFound();
                }
                notAddedUsers = GetNotAddedUsersToGroup(chat);

                bool addUsersToChat = await AddUsersToGroup(chatVM.Users, chat);

                ViewData["ApplicationUserId"] = new SelectList(notAddedUsers.OrderBy(u => u.Nume).ToList(), "Id", "Nume");
                return PartialView("_AddUsersToGroup");
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<bool> AddUsersToGroup(List<string> users, Chat chat)
        {
            if (users.Count() > 0 && chat != null)
            {
                foreach (var user in users)
                {
                    var chatUser = new ChatUser();
                    chatUser.ApplicationUserId = user;
                    chatUser.ChatId = chat.ChatId;

                    if (await _userManager.IsInRoleAsync(_context.ApplicationUsers.First(x => x.Id == user), ConstantVar.Rol_Admin))
                    {
                        chatUser.Role = UserChatRole.Admin;
                    }
                    else
                    {
                        chatUser.Role = UserChatRole.Membru;
                    }

                    chat.Users.Add(chatUser);
                    _context.ChatUsers.Add(chatUser);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private List<ApplicationUser> GetNotAddedUsersToGroup(Chat chat)
        {
            ICollection<ApplicationUser> addedUsers = new List<ApplicationUser>();
            ICollection<ApplicationUser> notAddedUsers = new List<ApplicationUser>();
            ICollection<ApplicationUser> users = _context.ApplicationUsers.ToList();

            if (chat != null)
            {
                List<ChatUser> chatUsers = _context.ChatUsers.Where(x => x.ChatId == chat.ChatId).ToList();

                foreach (var user in chatUsers)
                {
                    addedUsers.Add(_context.ApplicationUsers.First(x => x.Id == user.ApplicationUserId));
                }

                foreach (var user in users)
                {
                    if (!addedUsers.Contains(user))
                    {
                        notAddedUsers.Add(user);
                    }
                }
            }

            return notAddedUsers.ToList();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteGroup(int? id)
        {
            Chat chat = _context.Chats.Find(id.Value);
            var user = await _userManager.GetUserAsync(User);
            var chatUser = _context.ChatUsers.First(x => x.ApplicationUserId == user.Id);
            var chatToOpen = await _context.Chats.FirstOrDefaultAsync(x => x.Users.Contains(chatUser));

            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Grupul a fost sters cu succes!", chatId = chatToOpen.ChatId});
            } 
            else
            {
                return Json(new { success = false, message = "Grupul nu a fost gasit!" });
            }
        }
        #endregion

        #region
        // Delete user from given group
        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteGroupUser(string id, int chatId)
        {
            Chat chat = _context.Chats.Find(chatId);

            if (chat == null)
            {
                return Json(new { success = false, message = $"Grupul {chat.Nume} nu exista" });
            }

            ChatUser chatUser = _context.ChatUsers.Where(x => x.ChatId == chat.ChatId && x.ApplicationUserId == id).First();

            if (chatUser != null)
            {
                _context.ChatUsers.Remove(chatUser);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = $"Utilizator sters cu succes din grupul {chat.Nume}" });
            }

            return Json(new { success = false, message = $"Utilizator nu exista in grupul {chat.Nume}" });
        }
        #endregion

    }
}
