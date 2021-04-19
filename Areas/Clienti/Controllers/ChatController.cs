﻿using Licenta.Data;
using Licenta.Hubs;
using Licenta.Models;
using Licenta.Models.Chat;
using Licenta.Services.ChatManager;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        [HttpGet("[action]")]
        public IActionResult Chat()
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

            var chat = _context.Chats
                .Include(x => x.Mesaje)
                .Include(x => x.Users)
                    .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefault(x => x.ChatId == chatId);

            return View(chat);
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