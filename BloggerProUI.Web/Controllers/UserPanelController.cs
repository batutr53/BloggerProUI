using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using Microsoft.AspNetCore.Authorization;
using BloggerProUI.Business.Interfaces;

namespace BloggerProUI.Web.Controllers;

[Authorize]
public class UserPanelController : Controller
{
    private readonly ILogger<UserPanelController> _logger;
    private readonly IBookmarkApiService _bookmarkApiService;

    public UserPanelController(ILogger<UserPanelController> logger, IBookmarkApiService bookmarkApiService)
    {
        _logger = logger;
        _bookmarkApiService = bookmarkApiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
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
