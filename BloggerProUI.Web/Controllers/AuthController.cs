using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using BloggerProUI.Models.Auth;
using BloggerProUI.Business.Interfaces;

namespace BloggerProUI.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;
        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = new BloggerProUI.Models.Auth.LoginDto
            {
                Email = model.Username, // Eğer email ile giriş yapılıyorsa
                Password = model.Password
            };
            var result = await _authApiService.LoginAsync(loginDto);
            if (result.Success)
            {
                // Token'ı cookie'ye kaydet
                Response.Cookies.Append("access_token", result.Data, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
                // Token ile kullanıcı bilgisi decode edilip claim setlenebilir, örnek olarak sadece username ekleniyor
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            // Register işlemi için AuthApiService'de bir metot varsa burada kullanılmalı
            // Şimdilik örnek olarak başarısız döndürülüyor
            ModelState.AddModelError("", "Kayıt API entegrasyonu eklenmeli.");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
