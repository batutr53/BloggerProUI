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

    public async Task<DataResult<PostDetailDto>> GetPostByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"post/{id}");
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

    public async Task<Result> UpdatePostAsync(PostUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"post", dto);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> DeletePostAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"post/{id}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> UpdatePostStatusAsync(Guid postId, PostStatus status, DateTime? publishDate = null)
    {
        var payload = new
        {
            PostId = postId,
            Status = status,
            PublishDate = publishDate
        };
        var response = await _httpClient.PutAsJsonAsync($"post/status", payload);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> UpdatePostVisibilityAsync(Guid postId, PostVisibility visibility)
    {
        var payload = new { PostId = postId, Visibility = visibility };
        var response = await _httpClient.PutAsJsonAsync($"post/visibility", payload);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> TogglePostFeaturedStatusAsync(Guid postId)
    {
        var response = await _httpClient.PutAsync($"post/featured-toggle/{postId}", null);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<DataResult<PostModuleDto>> AddModuleToPostAsync(Guid postId, CreatePostModuleDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"post/{postId}", dto);
        return await response.Content.ReadFromJsonAsync<DataResult<PostModuleDto>>();
    }

    public async Task<DataResult<PostModuleDto>> UpdateModuleAsync(Guid postId, UpdatePostModuleDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"post/{postId}", dto);
        return await response.Content.ReadFromJsonAsync<DataResult<PostModuleDto>>();
    }

    public async Task<Result> RemoveModuleFromPostAsync(Guid postId, Guid moduleId)
    {
        var response = await _httpClient.DeleteAsync($"post/{postId}/modules/{moduleId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> ReorderModulesAsync(Guid postId, List<ModuleSortOrderDto> newOrder)
    {
        var response = await _httpClient.PutAsJsonAsync($"post/{postId}/modules/reorder", newOrder);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> LikePostAsync(Guid postId)
    {
        var response = await _httpClient.PostAsync($"post/like/{postId}", null);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> UnlikePostAsync(Guid postId)
    {
        var response = await _httpClient.DeleteAsync($"post/unlike/{postId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> RatePostAsync(Guid postId, int score)
    {
        var payload = new { Score = score };
        var response = await _httpClient.PostAsJsonAsync($"post/rate/{postId}", payload);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result> RemoveRatingAsync(Guid postId)
    {
        var response = await _httpClient.DeleteAsync($"post/unrate/{postId}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<bool> CanUserEditPostAsync(Guid postId)
    {
        var response = await _httpClient.GetAsync($"post/can-edit/{postId}");
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<bool> IsPostOwnerAsync(Guid postId)
    {
        var response = await _httpClient.GetAsync($"post/is-owner/{postId}");
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<bool> CanUserViewPostAsync(Guid postId)
    {
        var response = await _httpClient.GetAsync($"post/can-view/{postId}");
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<DataResult<PostStatsDto>> GetPostStatsAsync(Guid postId)
    {
        var response = await _httpClient.GetAsync($"post/{postId}/stats");
        return await response.Content.ReadFromJsonAsync<DataResult<PostStatsDto>>();
    }

    public async Task<DataResult<UserPostStatsDto>> GetUserPostStatsAsync()
    {
        var response = await _httpClient.GetAsync($"post/user-stat");
        return await response.Content.ReadFromJsonAsync<DataResult<UserPostStatsDto>>();
    }
}
