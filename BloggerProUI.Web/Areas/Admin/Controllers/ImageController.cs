using BloggerProUI.Business.Interfaces;
using BloggerProUI.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloggerProUI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class ImageController : Controller
    {
        private readonly IImageApiService _imageApiService;

        public ImageController(IImageApiService imageApiService)
        {
            _imageApiService = imageApiService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile upload)
        {
            if (upload == null || upload.Length == 0)
            {
                return Json(new { uploaded = false, error = new { message = "No file uploaded." } });
            }

            var result = await _imageApiService.UploadImageAsync(upload);

            if (result != null)
            {
                return Content(result, "application/json");
            }
            else
            {
                return Json(new { uploaded = false, error = new { message = "Image upload failed." } });
            }
        }
    }
}