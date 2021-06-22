﻿using Licenta.Data;
using Licenta.Models;
using Licenta.Models.Chat;
using Licenta.Services.ChatManager;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [HttpGet("[action]")]
        public IActionResult Find()
        {
            IList<ApplicationUser> users = _chatManager.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(users);
        }

        [HttpGet("[action]")]
        public IActionResult Private()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            IList<Chat> chats = _chatManager.Private(user.Id);
            return View(chats);
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

        [HttpPost("[action]/{name}")]
        public async Task<IActionResult> CreateRoom(string name)
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            if (await _chatManager.CreateRoom(name, user.Id))
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
                ApplicationUserId = _userManager.GetUserAsync(User).Result.Id,
                Role = UserChatRole.Admin
            });

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return PartialView("_AddChatRoom", chat);
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

            var utilizatoriPrivati = _context.Chats
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.Users.Any(y => y.ApplicationUserId == user.Id) && x.Tip.Equals(TipChat.Privat))
                .ToList();

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

        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            var chats = await _context.Chats
                    .Include(x => x.Users)
                        .ThenInclude(x => x.ApplicationUser)
                    .Where(x => !x.Users.Any(y => y.ApplicationUserId == user.Id))
                    .ToListAsync();

            return View(chats);
        }

        // Add grup modal
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
                Chat chat = new Chat
                {
                    Tip = TipChat.Grup,
                    Nume = chatRoomVM.Name
                };

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();

                foreach (var user in chatRoomVM.Users)
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
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers.OrderBy(u => u.Nume).ToList(), "Id", "Nume");
            return PartialView("_AddChatRoom");
        }
        #endregion
    }
}
