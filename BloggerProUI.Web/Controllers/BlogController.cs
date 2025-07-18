using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.Comment;

namespace BloggerProUI.Web.Controllers;

[Route("Blog")]
public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;
    private readonly IPostApiService _postApiService;
    private readonly ICommentApiService _commentApiService;
    private readonly ICategoryApiService _categoryApiService;
    private readonly ITagApiService _tagApiService;
    private readonly IBookmarkApiService _bookmarkApiService;

    public BlogController(ILogger<BlogController> logger, IPostApiService postApiService, ICommentApiService commentApiService, ICategoryApiService categoryApiService, ITagApiService tagApiService, IBookmarkApiService bookmarkApiService)
    {
        _logger = logger;
        _postApiService = postApiService;
        _commentApiService = commentApiService;
        _categoryApiService = categoryApiService;
        _tagApiService = tagApiService;
        _bookmarkApiService = bookmarkApiService;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? category = null, string? tag = null)
    {
        try
        {
            // Fetch posts, categories, and tags in parallel
            var postsTask = _postApiService.GetAllPostsAsync(page, pageSize);
            var categoriesTask = _categoryApiService.GetAllAsync();
            var tagsTask = _tagApiService.GetAllAsync();
            
            await Task.WhenAll(postsTask, categoriesTask, tagsTask);
            
            var postsResponse = postsTask.Result;
            var categoriesResponse = categoriesTask.Result;
            var tagsResponse = tagsTask.Result;
            
            // Pass additional data to view
            ViewBag.Categories = categoriesResponse?.Success == true ? categoriesResponse.Data : new List<BloggerProUI.Models.Category.CategoryDto>();
            ViewBag.Tags = tagsResponse?.Success == true ? tagsResponse.Data : new List<BloggerProUI.Models.Tag.TagDto>();
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentTag = tag;
            
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

    [HttpGet("Detail/{id}")]
    [HttpGet("Post/{id}")]  // Alternative route
    [HttpGet("{id:guid}")]  // SEO-friendly route
    [HttpGet("{slug}")]     // SEO-friendly slug route
    [HttpGet("{year:int:min(2020)}/{month:int:min(1):max(12)}/{slug}")]  // SEO-friendly date-based route
    [HttpGet("{category}/{slug}")]  // SEO-friendly category-based route
    public async Task<IActionResult> Detail(string id, string? category = null, int? year = null, int? month = null, string? slug = null)
    {
        try
        {
            Guid guid;
       
            if (Guid.TryParse(id, out guid))
            {
                // ID is a valid GUID
            }
            else if (!string.IsNullOrEmpty(slug))
            {
                // Try to find post by slug
                var postBySlugResponse = await _postApiService.GetPostBySlugAsync(slug);
                if (postBySlugResponse.Success && postBySlugResponse.Data != null)
                {
                    guid = postBySlugResponse.Data.Id;
                    
                    // Continue with the slug-based URL, don't redirect
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                // ID might be a slug - try to find post by slug
                var postBySlugResponse = await _postApiService.GetPostBySlugAsync(id);
                if (postBySlugResponse.Success && postBySlugResponse.Data != null)
                {
                    guid = postBySlugResponse.Data.Id;
                    
                    // Continue with the slug-based URL, don't redirect
                }
                else
                {
                    return NotFound();
                }
            }
            
            // Fetch post details, comments, related posts, and tags in parallel
            var postTask = _postApiService.GetPostByIdAsync(guid);
            var commentsTask = _commentApiService.GetCommentsByPostAsync(guid);
            var relatedPostsTask = _postApiService.GetAllPostsAsync(1, 3); // Get 3 related posts
            var tagsTask = _tagApiService.GetAllAsync(); // Get all tags for sidebar
            
            await Task.WhenAll(postTask, commentsTask, relatedPostsTask, tagsTask);
            
            var postResponse = postTask.Result;
            var commentsResponse = commentsTask.Result;
            var relatedPostsResponse = relatedPostsTask.Result;
            var tagsResponse = tagsTask.Result;
            
            if (postResponse?.Success == true && postResponse.Data != null)
            {
                // Increment view count (fire and forget - don't wait for response)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _postApiService.IncrementViewCountAsync(guid);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to increment view count for post {PostId}", guid);
                    }
                });

                // Get comments and update like status for each
                var comments = commentsResponse?.Success == true ? commentsResponse.Data : new List<CommentListDto>();
                
                // Update like status for each comment and its replies
                if (comments != null)
                {
                    foreach (var comment in comments)
                    {
                        // Check if current user has liked this comment
                        var hasLiked = await _commentApiService.HasUserLikedCommentAsync(comment.Id);
                        comment.HasLiked = hasLiked.Success && hasLiked.Data;
                        
                        // Update like status for replies
                        if (comment.Replies != null)
                        {
                            foreach (var reply in comment.Replies)
                            {
                                var replyHasLiked = await _commentApiService.HasUserLikedCommentAsync(reply.Id);
                                reply.HasLiked = replyHasLiked.Success && replyHasLiked.Data;
                            }
                        }
                    }
                }
                
                // Pass additional data to view
                ViewBag.Comments = comments;
                ViewBag.RelatedPosts = relatedPostsResponse?.Success == true ? relatedPostsResponse.Data?.Items?.Take(3).ToList() : new List<PostListDto>();
                ViewBag.AllTags = tagsResponse?.Success == true ? tagsResponse.Data : new List<BloggerProUI.Models.Tag.TagDto>();
                
                return View(postResponse.Data);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching post details");
            return NotFound();
        }
    }

    [HttpPost("LikePost/{id}")]
    public async Task<IActionResult> LikePost(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var postId))
            {
                return BadRequest();
            }
            
            var guid = Guid.Parse(id);
            var result = await _postApiService.LikePostAsync(guid);
            
            if (result.Success)
            {
                return Ok(new { success = true, message = "Post beğenildi" });
            }
            else
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Message ?? new[] { "Beğeni işlemi başarısız" }) });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while liking post");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpPost("UnlikePost/{id}")]
    public async Task<IActionResult> UnlikePost(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var postId))
            {
                return BadRequest();
            }
            
            var guid = Guid.Parse(id);
            var result = await _postApiService.UnlikePostAsync(guid);
            
            if (result.Success)
            {
                return Ok(new { success = true, message = "Beğeni kaldırıldı" });
            }
            else
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Message ?? new[] { "Beğeni kaldırma işlemi başarısız" }) });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while unliking post");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpPost("LikeComment/{id}")]
    public async Task<IActionResult> LikeComment(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var commentId))
            {
                return BadRequest(new { success = false, message = "Geçersiz yorum ID'si" });
            }
            
            var result = await _commentApiService.LikeCommentAsync(commentId);
            
            if (result.Success)
            {
                var likeCount = await _commentApiService.GetCommentLikeCountAsync(commentId);
                return Ok(new { 
                    success = true, 
                    message = "Yorum beğenildi",
                    likeCount = likeCount.Success ? likeCount.Data : 0,
                    hasLiked = true
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = string.Join(", ", result.Message ?? new[] { "Beğeni işlemi başarısız" }) 
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while liking comment");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpDelete("UnlikeComment/{id}")]
    public async Task<IActionResult> UnlikeComment(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var commentId))
            {
                return BadRequest(new { success = false, message = "Geçersiz yorum ID'si" });
            }
            
            var result = await _commentApiService.UnlikeCommentAsync(commentId);
            
            if (result.Success)
            {
                var likeCount = await _commentApiService.GetCommentLikeCountAsync(commentId);
                return Ok(new { 
                    success = true, 
                    message = "Beğeni kaldırıldı",
                    likeCount = likeCount.Success ? likeCount.Data : 0,
                    hasLiked = false
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = string.Join(", ", result.Message ?? new[] { "Beğeni kaldırma işlemi başarısız" }) 
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while unliking comment");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpGet("CommentLikes/{id}")]
    public async Task<IActionResult> GetCommentLikes(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var commentId))
            {
                return BadRequest(new { success = false, message = "Geçersiz yorum ID'si" });
            }
            
            var likeCount = await _commentApiService.GetCommentLikeCountAsync(commentId);
            var hasLiked = await _commentApiService.HasUserLikedCommentAsync(commentId);
            
            return Ok(new { 
                success = true, 
                likeCount = likeCount.Success ? likeCount.Data : 0,
                hasLiked = hasLiked.Success && hasLiked.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting comment likes");
            return BadRequest(new { success = false, message = "Beğeni bilgileri alınamadı" });
        }
    }

    [HttpPost("AddComment")]
    public async Task<IActionResult> AddComment([FromBody] CommentCreateDto commentDto)
    {
        try
        {
            if (commentDto == null)
            {
                return BadRequest(new { success = false, message = "Geçersiz yorum verisi" });
            }
            
            var result = await _commentApiService.AddCommentAsync(commentDto);
            
            if (result.Success)
            {
                return Ok(new { success = true, message = "Yorum eklendi", commentId = result.Data });
            }
            else
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Message ?? new[] { "Yorum eklenemedi" }) });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding comment");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpPost("DeleteComment/{id}")]
    public async Task<IActionResult> DeleteComment(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var commentId))
            {
                return BadRequest();
            }
            
            var guid = Guid.Parse(id);
            var result = await _commentApiService.DeleteCommentAsync(guid);
            
            if (result.Success)
            {
                return Ok(new { success = true, message = "Yorum silindi" });
            }
            else
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Message ?? new[] { "Yorum silinemedi" }) });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting comment");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpPost("AddBookmark")]
    public async Task<IActionResult> AddBookmark([FromBody] BloggerProUI.Models.Bookmark.BookmarkCreateDto bookmarkDto)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var result = await _bookmarkApiService.AddBookmarkAsync(bookmarkDto);

            if (result.Success)
            {
                return Ok(new { success = true, message = "Yazı favorilere eklendi" });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding bookmark");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpDelete("RemoveBookmark/{postId}")]
    public async Task<IActionResult> RemoveBookmark(Guid postId)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var result = await _bookmarkApiService.RemoveBookmarkAsync(postId);

            if (result.Success)
            {
                return Ok(new { success = true, message = "Yazı favorilerden kaldırıldı" });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while removing bookmark");
            return BadRequest(new { success = false, message = "Bir hata oluştu" });
        }
    }

    [HttpGet("IsBookmarked/{postId}")]
    public async Task<IActionResult> IsBookmarked(Guid postId)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Ok(new { data = false });
            }

            var result = await _bookmarkApiService.IsBookmarkedAsync(postId);

            return Ok(new { data = result.Data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking bookmark status");
            return Ok(new { data = false });
        }
    }

    [HttpGet("Search")]
    public async Task<IActionResult> SearchPosts(string keyword, int page = 1, int pageSize = 8)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { success = false, message = "Arama terimi gerekli" });
            }

            var result = await _postApiService.SearchPostsAsync(keyword, page, pageSize);
            
            if (result.Success)
            {
                return Ok(new { success = true, data = result.Data });
            }
            else
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Message ?? new[] { "Arama başarısız" }) });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching posts");
            return BadRequest(new { success = false, message = "Arama sırasında bir hata oluştu" });
        }
    }

}
