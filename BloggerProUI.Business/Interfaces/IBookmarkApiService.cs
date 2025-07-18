using BloggerProUI.Models.Bookmark;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IBookmarkApiService
    {
        Task<IResult> AddBookmarkAsync(BookmarkCreateDto bookmarkCreateDto);
        Task<IResult> RemoveBookmarkAsync(Guid postId);
        Task<IDataResult<bool>> IsBookmarkedAsync(Guid postId);
        Task<IDataResult<List<BookmarkListDto>>> GetMyBookmarksAsync();
        Task<IDataResult<int>> GetBookmarkCountAsync();
    }
}