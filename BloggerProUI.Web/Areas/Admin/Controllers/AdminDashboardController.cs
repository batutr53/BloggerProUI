using BloggerProUI.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Attributes;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class AdminDashboardController : Controller
{
    private readonly IAdminDashboardApiService _dashboardService;

    public AdminDashboardController(IAdminDashboardApiService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _dashboardService.GetStatsAsync();
            if (result.Success && result.Data != null)
            {
                return View(result.Data);
            }
            
            // API çağrısı başarısız olursa boş model oluştur
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Dashboard verileri alınamadı.";
            
            var emptyModel = new BloggerProUI.Models.Dashboard.AdminDashboardStatsDto
            {
                TotalUsers = 0,
                TotalPosts = 0,
                TotalComments = 0,
                TotalLikes = 0,
                TotalRatings = 0,
                TopLikedPosts = new List<BloggerProUI.Models.Post.PostDto>(),
                TopRatedPosts = new List<BloggerProUI.Models.Post.PostDto>(),
                MostActiveUsers = new List<BloggerProUI.Models.User.ActiveUserDto>()
            };
            
            return View(emptyModel);
        }
        catch (Exception ex)
        {
            // Hata durumunda boş model döndür
            var emptyModel = new BloggerProUI.Models.Dashboard.AdminDashboardStatsDto
            {
                TotalUsers = 0,
                TotalPosts = 0,
                TotalComments = 0,
                TotalLikes = 0,
                TotalRatings = 0,
                TopLikedPosts = new List<BloggerProUI.Models.Post.PostDto>(),
                TopRatedPosts = new List<BloggerProUI.Models.Post.PostDto>(),
                MostActiveUsers = new List<BloggerProUI.Models.User.ActiveUserDto>()
            };
            
            TempData["ErrorMessage"] = "Dashboard yüklenirken bir hata oluştu: " + ex.Message;
            return View(emptyModel);
        }
    }
}
