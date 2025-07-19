using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IUserApiService
    {
        Task<IDataResult<List<UserListDto>>> GetAllUsersWithRolesAsync();
        Task<IResult> UpdateUserRolesAsync(UpdateUserRolesDto dto);
        Task<IResult> ToggleUserBlockAsync(ToggleUserBlockDto dto);
        
        // Follow/Unfollow methods
        Task<IResult> FollowUserAsync(Guid userId);
        Task<IResult> UnfollowUserAsync(Guid userId);
        Task<IDataResult<bool>> IsFollowingAsync(Guid userId);
        Task<IDataResult<List<UserDto>>> GetFollowersAsync(Guid userId, int page = 1, int pageSize = 20);
        Task<IDataResult<List<UserDto>>> GetFollowingAsync(Guid userId, int page = 1, int pageSize = 20);
        Task<IDataResult<List<UserRecommendationDto>>> GetRecommendationsAsync(int limit = 10);
    }

}
