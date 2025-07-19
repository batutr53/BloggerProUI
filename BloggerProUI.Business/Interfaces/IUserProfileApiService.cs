using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;
using Microsoft.AspNetCore.Http;

namespace BloggerProUI.Business.Interfaces
{
    public interface IUserProfileApiService
    {
        Task<DataResult<UserProfileDto>> GetCurrentUserProfileAsync();
        Task<DataResult<UserProfileDto>> GetUserProfileAsync(Guid userId);
        Task<Result> UpdateProfileAsync(UpdateUserProfileDto dto);
        Task<Result> ChangePasswordAsync(ChangePasswordDto dto);
        Task<DataResult<string>> UploadProfileImageAsync(IFormFile file);
        Task<Result> FollowUserAsync(Guid userId);
        Task<Result> UnfollowUserAsync(Guid userId);
    }
}