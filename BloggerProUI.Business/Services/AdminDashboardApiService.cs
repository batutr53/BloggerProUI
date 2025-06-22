using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Dashboard;
using BloggerProUI.Shared.Utilities.Results;
using Newtonsoft.Json;

namespace BloggerProUI.Business.Services
{

    public class AdminDashboardApiService : IAdminDashboardApiService
    {
        private readonly HttpClient _httpClient;

        public AdminDashboardApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDataResult<AdminDashboardStatsDto>> GetStatsAsync()
        {
            var response = await _httpClient.GetAsync("AdminDashboard/stats");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<DataResult<AdminDashboardStatsDto>>(content);
                if (result != null && result.Success)
                    return result;
            }

            return new ErrorDataResult<AdminDashboardStatsDto>("Dashboard verileri alınamadı.");
        }
    }
}
