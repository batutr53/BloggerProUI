using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Models.Post;

namespace BloggerProUI.Web.Controllers;

[Route("Blog")]
public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;
    private readonly IPostApiService _postApiService;

    public BlogController(ILogger<BlogController> logger, IPostApiService postApiService)
    {
        _logger = logger;
        _postApiService = postApiService;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            // Get all posts from API
            var response = await _postApiService.GetAllPostsAsync(page, pageSize);
            
            if (response.Success && response.Data != null)
            {
                return View(response.Data);
            }
            else
            {
                // Log error and return empty result
                _logger.LogError("Failed to fetch posts: {Messages}", string.Join(", ", response.Message ?? new[] { "Unknown error" }));
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

    [HttpGet("Detail/{id}")]
    [HttpGet("Post/{id}")]  // Alternative route
    public async Task<IActionResult> Detail(string id)
    {
        
        try
        {
            var response = await _postApiService.GetPostByIdAsync(guidId);
            
            if (response?.Success == true && response.Data != null)
            {
                return View(response.Data);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }
}
