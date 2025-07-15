using BloggerProUI.Business.Interfaces;
using BloggerProUI.Business.Services;
using BloggerProUI.Models.Enums;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.PostModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class PostController : Controller
{
    private readonly IPostApiService _postApiService;
    private readonly ICategoryApiService _categoryApiService;
    private readonly ITagApiService _tagApiService;
    public PostController(IPostApiService postApiService, ICategoryApiService categoryApiService, ITagApiService tagApiService)
    {
        _postApiService = postApiService;
        _categoryApiService = categoryApiService;
        _tagApiService = tagApiService;
    }

    // GET: /Admin/Post
    public async Task<IActionResult> Index()
    {
        var result = await _postApiService.GetAllPostsAsync(1, 100);
        if (!result.Success) TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
        return View(result.Data?.Items ?? new List<PostListDto>());
    }

    // GET: /Admin/Post/Create
    public async Task<IActionResult> Create()
    {
        await PopulateViewBagForCreate();
        return View();
    }


    // POST: /Admin/Post/Create
    [HttpPost]
    public async Task<IActionResult> Create(PostCreateDto dto)
    {
        try
        {
            // File upload handling
            if (dto.FeaturedImageFile != null)
            {
                // Validate file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(dto.FeaturedImageFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["ErrorMessage"] = "Desteklenmeyen dosya formatı. Sadece JPG, PNG, GIF, WEBP dosyaları yükleyebilirsiniz.";
                    return View(dto);
                }

                if (dto.FeaturedImageFile.Length > 5 * 1024 * 1024) // 5MB limit
                {
                    TempData["ErrorMessage"] = "Dosya boyutu 5MB'dan büyük olamaz.";
                    return View(dto);
                }

                // Create unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadsPath = Path.Combine("wwwroot", "uploads", "posts");
                
                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var filePath = Path.Combine(uploadsPath, fileName);
                
                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.FeaturedImageFile.CopyToAsync(stream);
                }
                
                // Set the URL for the uploaded file
                dto.FeaturedImage = $"/uploads/posts/{fileName}";
            }

            // If no file uploaded and no URL provided, set a default or leave empty
            if (string.IsNullOrEmpty(dto.FeaturedImage))
            {
                dto.FeaturedImage = null; // API will handle this
            }

            var userId = GetCurrentUserId();
            var result = await _postApiService.CreatePostAsync(dto);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
                
                // Re-populate ViewBag for form
                await PopulateViewBagForCreate();
                return View(dto);
            }
            TempData["SuccessMessage"] = "Post başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Dosya yüklenirken bir hata oluştu: " + ex.Message;
            await PopulateViewBagForCreate();
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _postApiService.GetPostByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return RedirectToAction("Index");
        }

        var post = result.Data;

        var categoryList = await _categoryApiService.GetAllAsync();
        var tagList = await _tagApiService.GetAllAsync();

        var selectedCategoryIds = post.Categories.Select(x => x.ToString()).ToList();
        var selectedTagIds = post.Tags.Select(x => x.ToString()).ToList();

        ViewBag.Categories = categoryList.Data?
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = selectedCategoryIds.Contains(c.Id.ToString())
            }).ToList();

        ViewBag.Tags = tagList.Data?
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
                Selected = selectedTagIds.Contains(t.Id.ToString())
            }).ToList();

        return View(post);
    }


    // POST: /Admin/Post/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(PostUpdateDto dto)
    {
        if (dto.FeaturedImageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(dto.FeaturedImageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Desteklenmeyen dosya formatı. Sadece JPG, PNG, GIF, WEBP dosyaları yükleyebilirsiniz.";
                return View(dto);
            }
            if (dto.FeaturedImageFile.Length > 5 * 1024 * 1024) // 5MB limit
            {
                TempData["ErrorMessage"] = "Dosya boyutu 5MB'dan büyük olamaz.";
                return View(dto);
            }
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsPath = Path.Combine("wwwroot", "uploads", "posts");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            var filePath = Path.Combine(uploadsPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.FeaturedImageFile.CopyToAsync(stream);
            }
            dto.FeaturedImage = $"/uploads/posts/{fileName}";
        }

        dto.CoverImageUrl = dto.FeaturedImage;
        var result = await _postApiService.UpdatePostAsync(dto);
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
        var result = await _postApiService.DeletePostAsync(id);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/UpdateStatus
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(Guid id, PostStatus status, DateTime? publishDate = null)
    {
        var result = await _postApiService.UpdatePostStatusAsync(id, status, publishDate);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/UpdateVisibility
    [HttpPost]
    public async Task<IActionResult> UpdateVisibility(Guid id, PostVisibility visibility)
    {
        var result = await _postApiService.UpdatePostVisibilityAsync(id, visibility);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/ToggleFeatured
    [HttpPost]
    public async Task<IActionResult> ToggleFeatured(Guid id)
    {
        var result = await _postApiService.TogglePostFeaturedStatusAsync(id);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/AddModule
    [HttpPost]
    public async Task<IActionResult> AddModule(Guid postId, CreatePostModuleDto dto)
    {
        var result = await _postApiService.AddModuleToPostAsync(postId, dto);
        return Json(new { success = result.Success, data = result.Data, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/UpdateModule
    [HttpPost]
    public async Task<IActionResult> UpdateModule(Guid postId, UpdatePostModuleDto dto)
    {
        var result = await _postApiService.UpdateModuleAsync(postId, dto);
        return Json(new { success = result.Success, data = result.Data, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/DeleteModule
    [HttpPost]
    public async Task<IActionResult> DeleteModule(Guid postId, Guid moduleId)
    {
        var result = await _postApiService.RemoveModuleFromPostAsync(postId, moduleId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/ReorderModules
    [HttpPost]
    public async Task<IActionResult> ReorderModules(Guid postId, List<ModuleSortOrderDto> newOrder)
    {
        var result = await _postApiService.ReorderModulesAsync(postId, newOrder);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/Like
    [HttpPost]
    public async Task<IActionResult> Like(Guid postId)
    {
        var result = await _postApiService.LikePostAsync(postId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/Unlike
    [HttpPost]
    public async Task<IActionResult> Unlike(Guid postId)
    {
        var result = await _postApiService.UnlikePostAsync(postId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/Rate
    [HttpPost]
    public async Task<IActionResult> Rate(Guid postId, int score)
    {
        var result = await _postApiService.RatePostAsync(postId, score);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // POST: /Admin/Post/RemoveRating
    [HttpPost]
    public async Task<IActionResult> RemoveRating(Guid postId)
    {
        var result = await _postApiService.RemoveRatingAsync(postId);
        return Json(new { success = result.Success, message = result.Message?.FirstOrDefault() });
    }

    // GET: /Admin/Post/Stats/{postId}
    public async Task<IActionResult> Stats(Guid postId)
    {
        var result = await _postApiService.GetPostStatsAsync(postId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
            return RedirectToAction("Index");
        }
        return View(result.Data);
    }

    private async Task PopulateViewBagForCreate()
    {
        var categoryResult = await _categoryApiService.GetAllAsync();
        var tagResult = await _tagApiService.GetAllAsync();

        ViewBag.Categories = categoryResult.Data?
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList() ?? new List<SelectListItem>();

        ViewBag.Tags = tagResult.Data?
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList() ?? new List<SelectListItem>();
    }

    private Guid GetCurrentUserId()
    {
        var token = Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token))
            return Guid.Empty;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var id) ? id : Guid.Empty;

    }
}
