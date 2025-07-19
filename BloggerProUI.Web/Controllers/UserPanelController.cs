using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using Microsoft.AspNetCore.Authorization;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Business.Services;
using BloggerProUI.Models.User;

namespace BloggerProUI.Web.Controllers;

[Authorize]
public class UserPanelController : BaseController
{
    private readonly ILogger<UserPanelController> _logger;
    private readonly IBookmarkApiService _bookmarkApiService;
    private readonly UserDashboardApiService _userDashboardApiService;
    private readonly IUserProfileApiService _userProfileApiService;

    public UserPanelController(ILogger<UserPanelController> logger, IBookmarkApiService bookmarkApiService, UserDashboardApiService userDashboardApiService, IUserProfileApiService userProfileApiService)
    {
        _logger = logger;
        _bookmarkApiService = bookmarkApiService;
        _userDashboardApiService = userDashboardApiService;
        _userProfileApiService = userProfileApiService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var statsResult = await _userDashboardApiService.GetUserDashboardStatsAsync();
            var activitiesResult = await _userDashboardApiService.GetUserActivitiesAsync(10);
            var recentPostsResult = await _userDashboardApiService.GetRecentPostsAsync(5);
            var readingStatsResult = await _userDashboardApiService.GetReadingStatsAsync();

            var viewModel = new UserDashboardViewModel
            {
                Stats = statsResult.Success ? statsResult.Data : new BloggerProUI.Models.UserDashboard.UserDashboardStatsDto(),
                Activities = activitiesResult.Success ? activitiesResult.Data : new List<BloggerProUI.Models.UserDashboard.UserActivityDto>(),
                RecentPosts = recentPostsResult.Success ? recentPostsResult.Data : new List<BloggerProUI.Models.UserDashboard.RecentPostDto>(),
                ReadingStats = readingStatsResult.Success ? readingStatsResult.Data : new Dictionary<string, int>()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching dashboard data");
            return View(new UserDashboardViewModel());
        }
    }


    public IActionResult Reading()
    {
        return View();
    }

    public async Task<IActionResult> Bookmarks()
    {
        try
        {
            var result = await _bookmarkApiService.GetMyBookmarksAsync();
            
            if (result.Success)
            {
                return View(result.Data);
            }
            else
            {
                _logger.LogError("Failed to fetch bookmarks: {Message}", result.Message);
                return View(new List<BloggerProUI.Models.Bookmark.BookmarkListDto>());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching bookmarks");
            return View(new List<BloggerProUI.Models.Bookmark.BookmarkListDto>());
        }
    }

    public IActionResult Profile()
    {
        return View();
    }

    // Profile API Endpoints for AJAX calls
    [HttpGet]
    [Route("UserPanel/api/profile/current")]
    public async Task<IActionResult> GetCurrentProfile()
    {
        try
        {
            var result = await _userProfileApiService.GetCurrentUserProfileAsync();
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching current user profile");
            return Json(new { Success = false, Message = "Profil bilgileri alınırken hata oluştu." });
        }
    }

    [HttpPut]
    [Route("UserPanel/api/profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Message = "Geçersiz veri." });
            }

            var result = await _userProfileApiService.UpdateProfileAsync(dto);
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating profile");
            return Json(new { Success = false, Message = "Profil güncellenirken hata oluştu." });
        }
    }

    [HttpPatch]
    [Route("UserPanel/api/profile/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Message = "Geçersiz şifre bilgileri." });
            }

            var result = await _userProfileApiService.ChangePasswordAsync(dto);
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while changing password");
            return Json(new { Success = false, Message = "Şifre değiştirilirken hata oluştu." });
        }
    }

    [HttpPost]
    [Route("UserPanel/api/profile/upload-avatar")]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { Success = false, Message = "Dosya seçilmedi." });
            }

            // Validate file size (5MB max)
            if (file.Length > 5 * 1024 * 1024)
            {
                return Json(new { Success = false, Message = "Dosya boyutu 5MB'dan küçük olmalıdır." });
            }

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
            {
                return Json(new { Success = false, Message = "Sadece JPG, PNG, GIF ve WebP formatları desteklenir." });
            }

            var result = await _userProfileApiService.UploadProfileImageAsync(file);
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading profile image");
            return Json(new { Success = false, Message = "Profil resmi yüklenirken hata oluştu." });
        }
    }
}
