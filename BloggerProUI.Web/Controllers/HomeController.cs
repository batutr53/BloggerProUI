using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.Contact;

namespace BloggerProUI.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostApiService _postApiService;
    private readonly ICategoryApiService _categoryApiService;
    private readonly ITagApiService _tagApiService;
    private readonly ICommentApiService _commentApiService;
    private readonly IContactApiService _contactApiService;

    public HomeController(
        ILogger<HomeController> logger,
        IPostApiService postApiService,
        ICategoryApiService categoryApiService,
        ITagApiService tagApiService,
        ICommentApiService commentApiService,
        IContactApiService contactApiService)
    {
        _logger = logger;
        _postApiService = postApiService;
        _categoryApiService = categoryApiService;
        _tagApiService = tagApiService;
        _commentApiService = commentApiService;
        _contactApiService = contactApiService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            // Get posts, recent comments, and featured posts in parallel
            var postsTask = _postApiService.GetAllPostsAsync(page, pageSize);
            var commentsTask = _commentApiService.GetRecentCommentsAsync(5); // Get 5 recent comments
            var featuredPostsTask = _postApiService.GetFeaturedPostsAsync(3); // Get 3 featured posts

            await Task.WhenAll(postsTask, commentsTask, featuredPostsTask);

            var postsResponse = postsTask.Result;
            var commentsResponse = commentsTask.Result;
            var featuredPostsResponse = featuredPostsTask.Result;

            // Pass comments and featured posts to view via ViewBag
            ViewBag.RecentComments = commentsResponse?.Success == true ? commentsResponse.Data : new List<BloggerProUI.Models.Comment.RecentCommentDto>();
            ViewBag.FeaturedPosts = featuredPostsResponse?.Success == true ? featuredPostsResponse.Data : new List<PostListDto>();
            
            if (postsResponse.Success && postsResponse.Data != null)
            {
                return View(postsResponse.Data);
            }
            else
            {
                // Log error and return empty result
                _logger.LogError("Failed to fetch posts: {Messages}", string.Join(", ", postsResponse.Message ?? new[] { "Unknown error" }));
                var emptyResult = new PaginatedResultDto<PostListDto>();
                return View(emptyResult);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching posts");
            var emptyResult = new PaginatedResultDto<PostListDto>();
            return View(emptyResult);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactCreateDto contactCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(contactCreateDto);
        }

        try
        {
            var result = await _contactApiService.CreateContactAsync(contactCreateDto);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
                return RedirectToAction("Contact");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Mesaj gönderilirken bir hata oluştu.";
                return View(contactCreateDto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending contact message");
            TempData["ErrorMessage"] = "Mesaj gönderilirken beklenmeyen bir hata oluştu.";
            return View(contactCreateDto);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
