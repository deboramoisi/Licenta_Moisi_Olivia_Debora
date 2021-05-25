using Licenta.Models;
using Licenta.Services.NotificationManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Controllers
{
    [Authorize]
    [Area("Clienti")]

    public class NotificareController : Controller
    {
        private readonly INotificationManager _notificationManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificareController(INotificationManager notificationManager,
            UserManager<ApplicationUser> userManager)
        {
            _notificationManager = notificationManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetNotification()
        {
            var userId = _userManager.GetUserId(User);
            var notificare = await _notificationManager.GetNotificareUsers(userId);
            return Ok(new { NotificareUser = notificare, Count = notificare.Count});
        }

        [HttpGet]
        public IActionResult ReadNotification(int notificareId)
        {
            var userId = _userManager.GetUserId(User);
            _notificationManager.NotificationSeen(notificareId, userId);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetRedirectToPage(int id)
        {
            var redirectToPage = _notificationManager.GetRedirectToPageForNotification(id);
            return Json(new { redirectToPage });
        }
    }
}
