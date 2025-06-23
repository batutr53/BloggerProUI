using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Enums;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.PostModule;
using BloggerProUI.Models.User;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services;

public class PostApiService : IPostApiService
{
    private readonly HttpClient _httpClient;

    public PostApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataResult<string>> CreatePostAsync(PostCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"post", dto);
        return await response.Content.ReadFromJsonAsync<DataResult<string>>();
    }

    public async Task<DataResult<PostDetailDto>> GetPostByIdAsync(Guid id, Guid? userId = null)
    {
        var query = userId.HasValue ? $"?userId={userId}" : string.Empty;
        var response = await _httpClient.GetAsync($"post/{id}{query}");
        return await response.Content.ReadFromJsonAsync<DataResult<PostDetailDto>>();
    }

    public async Task<DataResult<PaginatedResultDto<PostListDto>>> GetAllPostsAsync(PostFilterDto filter, int page = 1, int pageSize = 10)
    {
        var response = await _httpClient.PostAsJsonAsync($"post/all?page={page}&pageSize={pageSize}", filter);
        return await response.Content.ReadFromJsonAsync<DataResult<PaginatedResultDto<PostListDto>>>();
    }
    public async Task<DataResult<PaginatedResultDto<PostListDto>>> GetAllPostsAsync(int page = 1, int pageSize = 10)
    {
        var response = await _httpClient.PostAsJsonAsync($"post/GetAllPost?page={page}&pageSize={pageSize}", "");
        return await response.Content.ReadFromJsonAsync<DataResult<PaginatedResultDto<PostListDto>>>();
    }
    public async Task<DataResult<PaginatedResultDto<PostListDto>>> GetPostsByAuthorIdAsync(Guid authorId, PostFilterDto filter, int page = 1, int pageSize = 10)
    {
        var response = await _httpClient.PostAsJsonAsync($"post/author/{authorId}?page={page}&pageSize={pageSize}", filter);
        return await response.Content.ReadFromJsonAsync<DataResult<PaginatedResultDto<PostListDto>>>();
    }

    public async Task<Result> UpdatePostAsync(PostUpdateDto dto, Guid userId)
    {
        var response = await _httpClient.PutAsJsonAsync($"post/update?userId={userId}", dto);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> DeletePostAsync(Guid id, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"post/{id}?userId={userId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> UpdatePostStatusAsync(Guid postId, PostStatus status, Guid userId, DateTime? publishDate = null)
    {
        var payload = new
        {
            PostId = postId,
            Status = status,
            PublishDate = publishDate
        };
        var response = await _httpClient.PutAsJsonAsync($"post/status?userId={userId}", payload);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> UpdatePostVisibilityAsync(Guid postId, PostVisibility visibility, Guid userId)
    {
        var payload = new { PostId = postId, Visibility = visibility };
        var response = await _httpClient.PutAsJsonAsync($"post/visibility?userId={userId}", payload);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> TogglePostFeaturedStatusAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.PutAsync($"post/featured-toggle/{postId}?userId={userId}", null);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<DataResult<PostModuleDto>> AddModuleToPostAsync(Guid postId, CreatePostModuleDto dto, Guid userId)
    {
        var response = await _httpClient.PostAsJsonAsync($"post/{postId}/modules?userId={userId}", dto);
        return await response.Content.ReadFromJsonAsync<DataResult<PostModuleDto>>();
    }

    public async Task<DataResult<PostModuleDto>> UpdateModuleAsync(Guid postId, UpdatePostModuleDto dto, Guid userId)
    {
        var response = await _httpClient.PutAsJsonAsync($"post/{postId}/modules?userId={userId}", dto);
        return await response.Content.ReadFromJsonAsync<DataResult<PostModuleDto>>();
    }

    public async Task<Result> RemoveModuleFromPostAsync(Guid postId, Guid moduleId, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"post/{postId}/modules/{moduleId}?userId={userId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> ReorderModulesAsync(Guid postId, List<ModuleSortOrderDto> newOrder, Guid userId)
    {
        var response = await _httpClient.PutAsJsonAsync($"post/{postId}/modules/reorder?userId={userId}", newOrder);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> LikePostAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.PostAsync($"post/like/{postId}?userId={userId}", null);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> UnlikePostAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"post/unlike/{postId}?userId={userId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> RatePostAsync(Guid postId, int score, Guid userId)
    {
        var payload = new { Score = score };
        var response = await _httpClient.PostAsJsonAsync($"post/rate/{postId}?userId={userId}", payload);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> RemoveRatingAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"post/unrate/{postId}?userId={userId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<bool> CanUserEditPostAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.GetAsync($"post/can-edit/{postId}?userId={userId}");
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<bool> IsPostOwnerAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.GetAsync($"post/is-owner/{postId}?userId={userId}");
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<bool> CanUserViewPostAsync(Guid postId, Guid? userId)
    {
        var query = userId.HasValue ? $"?userId={userId}" : "";
        var response = await _httpClient.GetAsync($"post/can-view/{postId}{query}");
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<DataResult<PostStatsDto>> GetPostStatsAsync(Guid postId, Guid userId)
    {
        var response = await _httpClient.GetAsync($"post/{postId}/stats?userId={userId}");
        return await response.Content.ReadFromJsonAsync<DataResult<PostStatsDto>>();
    }

    public async Task<DataResult<UserPostStatsDto>> GetUserPostStatsAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"post/user-stats/{userId}");
        return await response.Content.ReadFromJsonAsync<DataResult<UserPostStatsDto>>();
    }
}
