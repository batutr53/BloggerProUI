using BloggerProUI.Models.Dashboard;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IAdminDashboardApiService
    {
        Task<IDataResult<AdminDashboardStatsDto>> GetStatsAsync();
    }
}
