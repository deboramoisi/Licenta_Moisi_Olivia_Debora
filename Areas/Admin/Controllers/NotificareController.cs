using Licenta.Data;
using Licenta.Models.Notificari;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Licenta.Areas.Admin.Controllers
{
    public class NotificareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificareController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Notificare notificare, string userId)
        {
            if (ModelState.IsValid)
            {
                _context.Notificari.Add(notificare);
                await _context.SaveChangesAsync();

                var documente = _context.Document
                    .Include(x => x.TipDocument)
                    .Include(x => x.Client)
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.ApplicationUserId == userId)
                    .ToList();

                foreach (var document in documente)
                {
                    var notificareUser = new NotificareUser();
                    notificareUser.ApplicationUserId = userId;
                    notificareUser.NotificareId = notificare.NotificareId;

                }

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<List<NotificareUser>> GetNotificareUsers(string userId)
        {
            return await _context.NotificareUsers.Where(x => x.ApplicationUserId == userId)
                .Include(x => x.Notificare)
                .ToListAsync();
        } 

        private void NotificationSeen(int id)
        {
            var notificare = _context.Notificari.Find(id);
            // notificare = true;
            _context.Update(notificare);
            _context.SaveChanges();
        }

    }
}
