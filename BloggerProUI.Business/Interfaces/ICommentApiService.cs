using BloggerProUI.Models.Comment;

namespace BloggerProUI.Business.Interfaces
{
    public interface ICommentApiService
    {
        Task<DataResult<List<CommentListDto>>> GetCommentsByPostAsync(Guid postId);
        Task<DataResult<Guid>> AddCommentAsync(CommentCreateDto dto);
        Task<Result> DeleteCommentAsync(Guid commentId);
    }
}
