using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Enums;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.PostModule;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class PostController : Controller
{
    private readonly IPostApiService _postApiService;

    public PostController(IPostApiService postApiService)
    {
        _postApiService = postApiService;
    }

    // GET: /Admin/Post
    public async Task<IActionResult> Index()
    {
        var result = await _postApiService.GetAllPostsAsync(new PostFilterDto(), 1, 100);
        if (!result.Success) TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
        return View(result.Data?.Items ?? new List<PostListDto>());
    }

    // GET: /Admin/Post/Create
    public IActionResult Create() => View();

    // POST: /Admin/Post/Create
    [HttpPost]
    public async Task<IActionResult> Create(PostCreateDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.CreatePostAsync(dto, userId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return View(dto);
        }
        TempData["SuccessMessage"] = "Post başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }

    // GET: /Admin/Post/Edit/{id}
    public async Task<IActionResult> Edit(Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.GetPostByIdAsync(id, userId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return RedirectToAction("Index");
        }
        return View(result.Data);
    }

    // POST: /Admin/Post/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(PostUpdateDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.UpdatePostAsync(dto, userId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return View(dto);
        }
        TempData["SuccessMessage"] = "Post başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    // POST: /Admin/Post/Delete
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.DeletePostAsync(id, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/UpdateStatus
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(Guid id, PostStatus status, DateTime? publishDate = null)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.UpdatePostStatusAsync(id, status, userId, publishDate);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/UpdateVisibility
    [HttpPost]
    public async Task<IActionResult> UpdateVisibility(Guid id, PostVisibility visibility)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.UpdatePostVisibilityAsync(id, visibility, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/ToggleFeatured
    [HttpPost]
    public async Task<IActionResult> ToggleFeatured(Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.TogglePostFeaturedStatusAsync(id, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/AddModule
    [HttpPost]
    public async Task<IActionResult> AddModule(Guid postId, CreatePostModuleDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.AddModuleToPostAsync(postId, dto, userId);
        return Json(new { success = result.Success, data = result.Data, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/UpdateModule
    [HttpPost]
    public async Task<IActionResult> UpdateModule(Guid postId, UpdatePostModuleDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.UpdateModuleAsync(postId, dto, userId);
        return Json(new { success = result.Success, data = result.Data, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/DeleteModule
    [HttpPost]
    public async Task<IActionResult> DeleteModule(Guid postId, Guid moduleId)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.RemoveModuleFromPostAsync(postId, moduleId, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/ReorderModules
    [HttpPost]
    public async Task<IActionResult> ReorderModules(Guid postId, List<ModuleSortOrderDto> newOrder)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.ReorderModulesAsync(postId, newOrder, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/Like
    [HttpPost]
    public async Task<IActionResult> Like(Guid postId)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.LikePostAsync(postId, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/Unlike
    [HttpPost]
    public async Task<IActionResult> Unlike(Guid postId)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.UnlikePostAsync(postId, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/Rate
    [HttpPost]
    public async Task<IActionResult> Rate(Guid postId, int score)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.RatePostAsync(postId, score, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/RemoveRating
    [HttpPost]
    public async Task<IActionResult> RemoveRating(Guid postId)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.RemoveRatingAsync(postId, userId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // GET: /Admin/Post/Stats/{postId}
    public async Task<IActionResult> Stats(Guid postId)
    {
        var userId = GetCurrentUserId();
        var result = await _postApiService.GetPostStatsAsync(postId, userId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return RedirectToAction("Index");
        }
        return View(result.Data);
    }

    private Guid GetCurrentUserId()
    {
        // Cookie veya token'dan kullanıcıyı al (basit demo amaçlı, gerektiğinde değiştir)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var id) ? id : Guid.Empty;
    }
}
