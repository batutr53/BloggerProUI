using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Controllers;

public class HeaderController : BaseController
{
    public IActionResult GetAuthStatus()
    {
        return Json(new
        {
            isAuthenticated = User.Identity?.IsAuthenticated ?? false,
            userName = User.Identity?.Name ?? ""
        });
    }
}