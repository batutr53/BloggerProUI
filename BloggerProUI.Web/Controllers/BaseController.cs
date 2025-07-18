using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Controllers;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class BaseController : Controller
{
    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
    {
        // Her action'da cache headers'ı zorla set et
        Response.Headers.CacheControl = "no-cache, no-store, must-revalidate, private";
        Response.Headers.Pragma = "no-cache";
        Response.Headers.Expires = "Thu, 01 Jan 1970 00:00:00 GMT";
        Response.Headers.ETag = $"\"{Guid.NewGuid()}\"";
        Response.Headers.LastModified = DateTime.UtcNow.ToString("R");
        
        base.OnActionExecuting(context);
    }
}