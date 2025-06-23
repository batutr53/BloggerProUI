using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IUserApiService
    {
        Task<IDataResult<List<UserListDto>>> GetAllUsersWithRolesAsync();
        Task<IResult> UpdateUserRolesAsync(UpdateUserRolesDto dto);
        Task<IResult> ToggleUserBlockAsync(ToggleUserBlockDto dto);
    }

}
