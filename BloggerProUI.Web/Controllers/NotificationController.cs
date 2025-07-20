using BloggerProUI.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationApiService _notificationApiService;

        public NotificationController(INotificationApiService notificationApiService)
        {
            _notificationApiService = notificationApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _notificationApiService.GetUserNotificationsAsync();
            
            if (result.Success)
            {
                return View(result.Data);
            }

            TempData["Error"] = result.Message;
            return View(new List<BloggerProUI.Models.Notification.NotificationDto>());
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var result = await _notificationApiService.MarkAsReadAsync(id);
            
            if (result.Success)
            {
                return Json(new { success = true, message = result.Message });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _notificationApiService.MarkAllAsReadAsync();
            
            if (result.Success)
            {
                return Json(new { success = true, message = result.Message });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var result = await _notificationApiService.GetUnreadCountAsync();
            
            if (result.Success)
            {
                return Json(new { success = true, count = result.Data });
            }

            return Json(new { success = false, count = 0 });
        }
    }
}