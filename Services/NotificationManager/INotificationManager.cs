using Licenta.Models.Notificari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.NotificationManager
{
    public interface INotificationManager
    {
        Task CreateAsync(Notificare notificare, int clientId);
        Task CreateAsyncNotificationForAdmin(Notificare notificare, string adminId);
        Task CreateChatNotificationAsync(Notificare notificare, string userId);
        Task<List<NotificareUser>> GetNotificareUsers(string userId);
        void NotificationSeen(int id, string userId);
        string GetRedirectToPageForNotification(int id);
    }
}
