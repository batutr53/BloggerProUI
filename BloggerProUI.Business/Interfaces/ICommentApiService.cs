using BloggerProUI.Models.Comment;

namespace BloggerProUI.Business.Interfaces
{
    public interface ICommentApiService
    {
        Task<DataResult<List<CommentListDto>>> GetCommentsByPostAsync(Guid postId);
        Task<DataResult<Guid>> AddCommentAsync(CommentCreateDto dto);
        Task<Result> DeleteCommentAsync(Guid commentId);
        Task<DataResult<List<CommentListDto>>> GetMostLikedCommentsAsync(int count = 10);
        Task<DataResult<List<RecentCommentDto>>> GetRecentCommentsAsync(int count = 5);
        
        // Like related methods
        Task<Result> LikeCommentAsync(Guid commentId);
        Task<Result> UnlikeCommentAsync(Guid commentId);
        Task<DataResult<int>> GetCommentLikeCountAsync(Guid commentId);
        Task<DataResult<bool>> HasUserLikedCommentAsync(Guid commentId);
    }
}
