using BloggerProUI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminDashboardController : Controller
{
    private readonly IAdminDashboardApiService _dashboardService;

    public AdminDashboardController(IAdminDashboardApiService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _dashboardService.GetStatsAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return View(null);
        }

        return View(result.Data);
    }
}
