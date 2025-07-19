using BloggerProUI.Models.Chat;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IPresenceApiService
    {
        Task<Result> UpdatePresenceAsync(bool isOnline);
        Task<DataResult<UserPresenceDto>> GetUserPresenceAsync(Guid userId);
        Task<DataResult<List<UserPresenceDto>>> GetMultipleUserPresenceAsync(List<Guid> userIds);
    }
}