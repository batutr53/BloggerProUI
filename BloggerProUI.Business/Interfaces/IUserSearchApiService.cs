using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IUserSearchApiService
    {
        Task<DataResult<List<UserSearchDto>>> SearchUsersAsync(string query, bool includeMutual = true, int limit = 20);
        Task<DataResult<List<UserRecommendationDto>>> GetRecommendationsAsync(int limit = 10);
        Task<DataResult<List<UserDto>>> GetMutualConnectionsAsync(Guid userId, Guid otherUserId);
    }
}