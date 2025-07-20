using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BloggerProUI.Web.Attributes
{
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            
            // Check if user is authenticated
            if (!user.Identity.IsAuthenticated)
            {
                // Redirect to admin login
                context.Result = new RedirectToActionResult("Login", "Auth", new { area = "Admin" });
                return;
            }
            
            // Check if user has Admin role
            if (!user.IsInRole("Admin"))
            {
                // Redirect to admin login if not admin
                context.Result = new RedirectToActionResult("Login", "Auth", new { area = "Admin" });
                return;
            }
        }
    }
}