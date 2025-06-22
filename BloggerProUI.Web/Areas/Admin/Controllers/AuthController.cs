using BloggerProUI.Models.Auth;
using BloggerProUI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authApiService.LoginAsync(dto);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(dto);
            }

            // Token cookie'ye yazılıyor
            Response.Cookies.Append("AuthToken", result.Data, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
    }
}
