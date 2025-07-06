using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;

namespace BloggerProUI.Web.Controllers;

public class UserPanelController : Controller
{
    private readonly ILogger<UserPanelController> _logger;

    public UserPanelController(ILogger<UserPanelController> logger)
    {
        _logger = logger;
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

    public IActionResult Bookmarks()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }
}
