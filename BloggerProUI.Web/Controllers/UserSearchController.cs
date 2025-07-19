using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.User;

namespace BloggerProUI.Web.Controllers
{
    [Authorize]
    public class UserSearchController : BaseController
    {
        private readonly IUserSearchApiService _userSearchApiService;
        private readonly IUserApiService _userApiService;

        public UserSearchController(
            IUserSearchApiService userSearchApiService,
            IUserApiService userApiService)
        {
            _userSearchApiService = userSearchApiService;
            _userApiService = userApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string q, bool mutual = true, int limit = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return Json(new { success = false, data = new List<UserSearchDto>() });
                }

                var result = await _userSearchApiService.SearchUsersAsync(q, mutual, limit);
                return Json(new { success = result.Success, data = result.Data ?? new List<UserSearchDto>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<UserSearchDto>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Recommendations(int limit = 10)
        {
            try
            {
                var result = await _userSearchApiService.GetRecommendationsAsync(limit);
                return Json(new { success = result.Success, data = result.Data ?? new List<UserRecommendationDto>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<UserRecommendationDto>() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Follow(Guid userId)
        {
            try
            {
                var result = await _userApiService.FollowUserAsync(userId);
                return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Takip etme işleminde hata oluştu." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(Guid userId)
        {
            try
            {
                var result = await _userApiService.UnfollowUserAsync(userId);
                return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Takip bırakma işleminde hata oluştu." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> IsFollowing(Guid userId)
        {
            try
            {
                var result = await _userApiService.IsFollowingAsync(userId);
                return Json(new { success = result.Success, data = result.Data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Followers(Guid userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var result = await _userApiService.GetFollowersAsync(userId, page, pageSize);
                return Json(new { success = result.Success, data = result.Data ?? new List<UserDto>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<UserDto>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Following(Guid userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var result = await _userApiService.GetFollowingAsync(userId, page, pageSize);
                return Json(new { success = result.Success, data = result.Data ?? new List<UserDto>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<UserDto>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mutuals(Guid otherUserId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _userSearchApiService.GetMutualConnectionsAsync(currentUserId, otherUserId);
                return Json(new { success = result.Success, data = result.Data ?? new List<UserDto>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<UserDto>() });
            }
        }
    }
}