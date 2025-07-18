using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggerProUI.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.Contact;
using BloggerProUI.Models.AboutUs;
using BloggerProUI.Models.TeamMember;

namespace BloggerProUI.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostApiService _postApiService;
    private readonly ICategoryApiService _categoryApiService;
    private readonly ITagApiService _tagApiService;
    private readonly ICommentApiService _commentApiService;
    private readonly IContactApiService _contactApiService;
    private readonly IAboutUsApiService _aboutUsApiService;
    private readonly ITeamMemberApiService _teamMemberApiService;
    private readonly IFooterApiService _footerApiService;
    public HomeController(
        ILogger<HomeController> logger,
        IPostApiService postApiService,
        ICategoryApiService categoryApiService,
        ITagApiService tagApiService,
        ICommentApiService commentApiService,
        IContactApiService contactApiService,
        IAboutUsApiService aboutUsApiService,
        ITeamMemberApiService teamMemberApiService,
        IFooterApiService footerApiService)
    {
        _logger = logger;
        _postApiService = postApiService;
        _categoryApiService = categoryApiService;
        _tagApiService = tagApiService;
        _commentApiService = commentApiService;
        _contactApiService = contactApiService;
        _aboutUsApiService = aboutUsApiService;
        _teamMemberApiService = teamMemberApiService;
        _footerApiService = footerApiService;
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

    public async Task<IActionResult> About()
    {
        try
        {
            // Get AboutUs and TeamMember data in parallel
            var aboutUsTask = _aboutUsApiService.GetAllAboutUsAsync();
            var teamMembersTask = _teamMemberApiService.GetAllTeamMembersAsync();

            await Task.WhenAll(aboutUsTask, teamMembersTask);

            var aboutUsResponse = aboutUsTask.Result;
            var teamMembersResponse = teamMembersTask.Result;

            // Create a view model
            var viewModel = new AboutPageViewModel
            {
                AboutUs = aboutUsResponse?.Success == true && aboutUsResponse.Data != null && aboutUsResponse.Data.Any() 
                    ? aboutUsResponse.Data.Where(a => a.IsActive).OrderBy(a => a.SortOrder).FirstOrDefault()
                    : null,
                TeamMembers = teamMembersResponse?.Success == true ? 
                    teamMembersResponse.Data?.Where(t => t.IsActive).OrderBy(t => t.SortOrder).ToList() ?? new List<TeamMemberDto>()
                    : new List<TeamMemberDto>()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching about page data");
            // Return view with null model to show fallback content
            return View(new AboutPageViewModel());
        }
    }

    public async Task<IActionResult> Contact()
    {
        await LoadContactInfoAsync();
        return View(new ContactCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateContact(ContactCreateDto contactCreateDto)
    {
        _logger.LogInformation("POST CreateContact metodu çağrıldı!");
        _logger.LogInformation("Model: Name={Name}, Email={Email}, Subject={Subject}", contactCreateDto?.Name, contactCreateDto?.Email, contactCreateDto?.Subject);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("ModelState geçersiz!");
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            
            return Json(new { success = false, message = "Form alanlarında hatalar var.", errors = errors });
        }

        try
        {
            _logger.LogInformation("Contact mesajı gönderiliyor: {Name}, {Email}, {Subject}", contactCreateDto.Name, contactCreateDto.Email, contactCreateDto.Subject);
            
            var result = await _contactApiService.CreateContactAsync(contactCreateDto);

            _logger.LogInformation("Contact API yanıtı: Success={Success}, Messages={Messages}", result.Success, string.Join(", ", result.Message ?? new string[0]));

            if (result.Success)
            {
                return Json(new { success = true, message = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız." });
            }
            else
            {
                return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "Mesaj gönderilirken bir hata oluştu." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending contact message");
            return Json(new { success = false, message = "Mesaj gönderilirken beklenmeyen bir hata oluştu." });
        }
    }

    private async Task LoadContactInfoAsync()
    {
        try
        {
            var footerResponse = await _footerApiService.GetAllFootersAsync();
            ViewBag.ContactInfo = footerResponse?.Success == true ? 
                footerResponse.Data?.Where(f => f.IsActive && f.FooterType == "ContactInfo").OrderBy(f => f.SortOrder).ToList() 
                : new List<BloggerProUI.Models.Footer.FooterDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading contact info");
            ViewBag.ContactInfo = new List<BloggerProUI.Models.Footer.FooterDto>();
        }
    }

    public async Task<IActionResult> Search(string keyword, int page = 1, int pageSize = 10)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Json(new { success = false, message = "Arama kelimesi boş olamaz." });
            }

            var result = await _postApiService.SearchPostsAsync(keyword, page, pageSize);
            
            if (result.Success && result.Data != null)
            {
                return Json(new { 
                    success = true, 
                    data = result.Data.Items,
                    //totalCount = result.Data.TotalCount,
                    //currentPage = result.Data.CurrentPage,
                    totalPages = result.Data.TotalPages
                });
            }
            else
            {
                return Json(new { success = false, message = "Arama sonucu bulunamadı." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching posts");
            return Json(new { success = false, message = "Arama sırasında bir hata oluştu." });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
