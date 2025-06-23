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
        Task<DataResult<PostDetailDto>> GetPostByIdAsync(Guid id, Guid? userId = null);
        Task<DataResult<PaginatedResultDto<PostListDto>>> GetPostsByAuthorIdAsync(Guid authorId, PostFilterDto filter, int page = 1, int pageSize = 10);
        Task<DataResult<PaginatedResultDto<PostListDto>>> GetAllPostsAsync(PostFilterDto filter, int page = 1, int pageSize = 10);
        Task<Result> UpdatePostAsync(PostUpdateDto dto, Guid userId);
        Task<Result> DeletePostAsync(Guid id, Guid userId);

        Task<Result> UpdatePostStatusAsync(Guid postId, PostStatus status, Guid userId, DateTime? publishDate = null);
        Task<Result> UpdatePostVisibilityAsync(Guid postId, PostVisibility visibility, Guid userId);
        Task<Result> TogglePostFeaturedStatusAsync(Guid postId, Guid userId);

        Task<DataResult<PostModuleDto>> AddModuleToPostAsync(Guid postId, CreatePostModuleDto dto, Guid userId);
        Task<DataResult<PostModuleDto>> UpdateModuleAsync(Guid postId, UpdatePostModuleDto dto, Guid userId);
        Task<Result> RemoveModuleFromPostAsync(Guid postId, Guid moduleId, Guid userId);
        Task<Result> ReorderModulesAsync(Guid postId, List<ModuleSortOrderDto> newOrder, Guid userId);

        Task<Result> LikePostAsync(Guid postId, Guid userId);
        Task<Result> UnlikePostAsync(Guid postId, Guid userId);
        Task<Result> RatePostAsync(Guid postId, int score, Guid userId);
        Task<Result> RemoveRatingAsync(Guid postId, Guid userId);

        Task<bool> CanUserEditPostAsync(Guid postId, Guid userId);
        Task<bool> IsPostOwnerAsync(Guid postId, Guid userId);
        Task<bool> CanUserViewPostAsync(Guid postId, Guid? userId);

        Task<DataResult<PostStatsDto>> GetPostStatsAsync(Guid postId, Guid userId);
        Task<DataResult<UserPostStatsDto>> GetUserPostStatsAsync(Guid userId);
    }

}
