using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloggerProUI.Business.Interfaces;

namespace BloggerProUI.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostApiService _postApiService;
    private readonly ICategoryApiService _categoryApiService;
    private readonly ITagApiService _tagApiService;

    public HomeController(
        ILogger<HomeController> logger,
        IPostApiService postApiService,
        ICategoryApiService categoryApiService,
        ITagApiService tagApiService)
    {
        _logger = logger;
        _postApiService = postApiService;
        _categoryApiService = categoryApiService;
        _tagApiService = tagApiService;

    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // Fetch data in parallel
            var featuredPostTask = _postApiService.GetAllPostsAsync();
            var categoriesTask = _categoryApiService.GetAllAsync();

            await Task.WhenAll(featuredPostTask, categoriesTask);

            // Use ViewData to pass data to the view
            ViewData["FeaturedPost"] = featuredPostTask.Result;
            ViewData["Categories"] = categoriesTask.Result;

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page data");
            // In a production app, you might want to show a friendly error page
            throw;
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
