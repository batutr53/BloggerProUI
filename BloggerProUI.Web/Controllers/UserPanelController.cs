using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using Microsoft.AspNetCore.Authorization;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Business.Services;

namespace BloggerProUI.Web.Controllers;

[Authorize]
public class UserPanelController : BaseController
{
    private readonly ILogger<UserPanelController> _logger;
    private readonly IBookmarkApiService _bookmarkApiService;
    private readonly UserDashboardApiService _userDashboardApiService;

    public UserPanelController(ILogger<UserPanelController> logger, IBookmarkApiService bookmarkApiService, UserDashboardApiService userDashboardApiService)
    {
        _logger = logger;
        _bookmarkApiService = bookmarkApiService;
        _userDashboardApiService = userDashboardApiService;
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
}
