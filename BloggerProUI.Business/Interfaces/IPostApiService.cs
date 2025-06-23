using BloggerProUI.Models.Enums;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Models.Post;
using BloggerProUI.Models.PostModule;
using BloggerProUI.Models.User;

namespace BloggerProUI.Business.Interfaces
{
    public interface IPostApiService
    {
        Task<DataResult<PaginatedResultDto<PostListDto>>> GetAllPostsAsync(int page = 1, int pageSize = 20);
        Task<DataResult<string>> CreatePostAsync(PostCreateDto dto);
        Task<DataResult<PostDetailDto>> GetPostByIdAsync(Guid id);
        Task<DataResult<PaginatedResultDto<PostListDto>>> GetPostsByAuthorIdAsync(Guid authorId, PostFilterDto filter, int page = 1, int pageSize = 10);
        Task<DataResult<PaginatedResultDto<PostListDto>>> GetAllPostsAsync(PostFilterDto filter, int page = 1, int pageSize = 10);
        Task<Result> UpdatePostAsync(PostUpdateDto dto);
        Task<Result> DeletePostAsync(Guid id);

        Task<Result> UpdatePostStatusAsync(Guid postId, PostStatus status, DateTime? publishDate = null);
        Task<Result> UpdatePostVisibilityAsync(Guid postId, PostVisibility visibility);
        Task<Result> TogglePostFeaturedStatusAsync(Guid postId);

        Task<DataResult<PostModuleDto>> AddModuleToPostAsync(Guid postId, CreatePostModuleDto dto);
        Task<DataResult<PostModuleDto>> UpdateModuleAsync(Guid postId, UpdatePostModuleDto dto);
        Task<Result> RemoveModuleFromPostAsync(Guid postId, Guid moduleId);
        Task<Result> ReorderModulesAsync(Guid postId, List<ModuleSortOrderDto> newOrder);

        Task<Result> LikePostAsync(Guid postId);
        Task<Result> UnlikePostAsync(Guid postId);
        Task<Result> RatePostAsync(Guid postId, int score);
        Task<Result> RemoveRatingAsync(Guid postId);

        Task<bool> CanUserEditPostAsync(Guid postId);
        Task<bool> IsPostOwnerAsync(Guid postId);
        Task<bool> CanUserViewPostAsync(Guid postId);

        Task<DataResult<PostStatsDto>> GetPostStatsAsync(Guid postId);
        Task<DataResult<UserPostStatsDto>> GetUserPostStatsAsync();
    }

}
