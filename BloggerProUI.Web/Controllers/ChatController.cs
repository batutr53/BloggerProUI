using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Chat;
using System.Security.Claims;

namespace BloggerProUI.Web.Controllers
{
    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IChatApiService _chatApiService;
        private readonly IUserApiService _userApiService;
        private readonly IPresenceApiService _presenceApiService;

        public ChatController(
            IChatApiService chatApiService,
            IUserApiService userApiService,
            IPresenceApiService presenceApiService)
        {
            _chatApiService = chatApiService;
            _userApiService = userApiService;
            _presenceApiService = presenceApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var conversations = await _chatApiService.GetConversationsAsync();
                return View(conversations.Data ?? new List<ConversationDto>());
            }
            catch (Exception ex)
            {
                return View(new List<ConversationDto>());
            }
        }

        public async Task<IActionResult> Conversation(Guid userId)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
                
                var canChat = await _chatApiService.CanUsersStartConversationAsync(currentUserId, userId);
                if (!canChat.Success || !canChat.Data)
                {
                    TempData["Error"] = "Bu kullanıcı ile sohbet edemezsiniz. Karşılıklı takip etmeniz gerekir.";
                    return RedirectToAction("Index");
                }

                var messages = await _chatApiService.GetMessagesAsync(userId);
                ViewBag.ReceiverUserId = userId;
                return View(messages.Data ?? new List<MessageDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Sohbet yüklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageDto dto)
        {
            try
            {
                var result = await _chatApiService.SendMessageAsync(dto);
                return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mesaj gönderilirken hata oluştu." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkMessagesAsReadDto dto)
        {
            try
            {
                var result = await _chatApiService.MarkMessagesAsReadAsync(dto);
                return Json(new { success = result.Success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid userId, int page = 1, int pageSize = 50)
        {
            try
            {
                var result = await _chatApiService.GetMessagesAsync(userId, page, pageSize);
                return Json(new { success = result.Success, data = result.Data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<MessageDto>() });
            }
        }
    }
}