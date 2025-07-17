using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentApiService _commentApiService;

        public CommentController(ICommentApiService commentApiService)
        {
            _commentApiService = commentApiService;
        }

        public async Task<IActionResult> Index(Guid postId)
        {
            var result = await _commentApiService.GetCommentsByPostAsync(postId);
            if (!result.Success) return View("Error", result.Message);

            ViewBag.PostId = postId;
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CommentCreateDto dto)
        {
            var result = await _commentApiService.AddCommentAsync(dto);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction("Index", new { postId = dto.PostId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, Guid postId)
        {
            var result = await _commentApiService.DeleteCommentAsync(id);
            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }
    }

}
