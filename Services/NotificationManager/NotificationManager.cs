using Licenta.Data;
using Licenta.Hubs;
using Licenta.Models.Notificari;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.NotificationManager
{
    public class NotificationManager : INotificationManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        
        public NotificationManager(ApplicationDbContext context,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CreateAsync(Notificare notificare, int clientId)
        {
            _context.Notificari.Add(notificare);
            _context.SaveChanges();

            var users = _context.ApplicationUsers
                .Include(x => x.Client)
                .Where(x => x.ClientId == clientId)
                .ToList();

            foreach (var user in users)
            {
                var notificareUser = new NotificareUser() { };
                notificareUser.ApplicationUserId = user.Id;
                notificareUser.NotificareId = notificare.NotificareId;

                _context.NotificareUsers.Add(notificareUser);
            }

            _context.SaveChanges();

            // execute client side method from site.js
            await _hubContext.Clients.All.SendAsync("displayNotification", "");
        }

        public async Task CreateAsyncNotificationForAdmin(Notificare notificare, string adminId)
        {
            _context.Notificari.Add(notificare);
            _context.SaveChanges();

            var notificareUser = new NotificareUser() { };
            notificareUser.ApplicationUserId = adminId;
            notificareUser.NotificareId = notificare.NotificareId;

            _context.NotificareUsers.Add(notificareUser);
            _context.SaveChanges();

            // execute client side method from site.js
            await _hubContext.Clients.All.SendAsync("displayNotification", "");
        }

        public async Task CreateChatNotificationAsync(Notificare notificare, string userId)
        {
            _context.Notificari.Add(notificare);
            _context.SaveChanges();

            var notificareUser = new NotificareUser() { };
            notificareUser.ApplicationUserId = userId;
            notificareUser.NotificareId = notificare.NotificareId;

            _context.NotificareUsers.Add(notificareUser);
            _context.SaveChanges();

            // execute client side method from site.js
            await _hubContext.Clients.All.SendAsync("displayNotification", "");
        }

        public async Task<List<NotificareUser>> GetNotificareUsers(string userId)
        {
            return await _context.NotificareUsers.Where(x => x.ApplicationUserId == userId)
                .Include(x => x.Notificare)
                .Where(x => !x.Seen)
                .ToListAsync();
        }

        public void NotificationSeen(int id, string userId)
        {
            var notificareUser = _context.NotificareUsers
                .Where(x => x.ApplicationUserId == userId && x.NotificareId == id)
                .FirstOrDefault();
            notificareUser.Seen = true;
            _context.NotificareUsers.Update(notificareUser);
            _context.NotificareUsers.Remove(notificareUser);
            _context.SaveChanges();
        }

        public string GetRedirectToPageForNotification(int id)
        {
            var notificare = _context.Notificari.FirstOrDefault(x => x.NotificareId == id);
            return notificare.RedirectToPage;
        }
    }
}
