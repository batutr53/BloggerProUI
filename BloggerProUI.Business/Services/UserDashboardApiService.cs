using BloggerProUI.Models.UserDashboard;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;
using System.Text.Json;

namespace BloggerProUI.Business.Services
{
    public class UserDashboardApiService
    {
        private readonly HttpClient _httpClient;

        public UserDashboardApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResult<UserDashboardStatsDto>> GetUserDashboardStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("UserDashboard/stats");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<UserDashboardStatsDto>("Dashboard istatistikleri alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<UserDashboardStatsDto>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<UserDashboardStatsDto>>();
                return result ?? new ErrorDataResult<UserDashboardStatsDto>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<UserDashboardStatsDto>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserDashboardStatsDto>("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<List<UserActivityDto>>> GetUserActivitiesAsync(int take = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserDashboard/activities?take={take}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<List<UserActivityDto>>("Kullanıcı aktiviteleri alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<List<UserActivityDto>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<UserActivityDto>>>();
                return result ?? new ErrorDataResult<List<UserActivityDto>>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<List<UserActivityDto>>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserActivityDto>>("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<List<RecentPostDto>>> GetRecentPostsAsync(int take = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserDashboard/recent-posts?take={take}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<List<RecentPostDto>>("Son okunan yazılar alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<List<RecentPostDto>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<RecentPostDto>>>();
                return result ?? new ErrorDataResult<List<RecentPostDto>>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<List<RecentPostDto>>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<RecentPostDto>>("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<List<ReadingSessionDto>>> GetActiveReadingSessionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("UserDashboard/active-sessions");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<List<ReadingSessionDto>>("Aktif okuma oturumları alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<List<ReadingSessionDto>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<ReadingSessionDto>>>();
                return result ?? new ErrorDataResult<List<ReadingSessionDto>>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<List<ReadingSessionDto>>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<ReadingSessionDto>>("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<Dictionary<string, int>>> GetReadingStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("UserDashboard/reading-stats");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<Dictionary<string, int>>("Okuma istatistikleri alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<Dictionary<string, int>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<Dictionary<string, int>>>();
                return result ?? new ErrorDataResult<Dictionary<string, int>>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<Dictionary<string, int>>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Dictionary<string, int>>("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<Dictionary<string, int>>> GetMonthlyReadingStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("UserDashboard/monthly-stats");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<Dictionary<string, int>>("Aylık okuma istatistikleri alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<Dictionary<string, int>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<Dictionary<string, int>>>();
                return result ?? new ErrorDataResult<Dictionary<string, int>>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<Dictionary<string, int>>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Dictionary<string, int>>("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<List<string>>> GetFavoriteCategoriesAsync(int take = 5)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserDashboard/favorite-categories?take={take}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<List<string>>("Favori kategoriler alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<List<string>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<string>>>();
                return result ?? new ErrorDataResult<List<string>>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<List<string>>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<string>>("Bir hata oluştu.");
            }
        }

        public async Task<Result> TrackPostViewAsync(Guid postId)
        {
            try
            {
                var request = new { PostId = postId };
                var response = await _httpClient.PostAsJsonAsync("UserDashboard/track-view", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorResult("Yazı görüntüleme kaydedilemedi.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result ?? new ErrorResult("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorResult("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Bir hata oluştu.");
            }
        }

        public async Task<DataResult<ReadingSessionDto>> StartReadingSessionAsync(Guid postId, string deviceType, string referrerUrl = "")
        {
            try
            {
                var request = new { PostId = postId, DeviceType = deviceType, ReferrerUrl = referrerUrl };
                var response = await _httpClient.PostAsJsonAsync("UserDashboard/start-session", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<ReadingSessionDto>("Okuma oturumu başlatılamadı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<ReadingSessionDto>>();
                return result ?? new ErrorDataResult<ReadingSessionDto>("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorDataResult<ReadingSessionDto>("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ReadingSessionDto>("Bir hata oluştu.");
            }
        }

        public async Task<Result> UpdateReadingSessionAsync(Guid postId, int readingTimeSeconds, int scrollPercentage)
        {
            try
            {
                var request = new { PostId = postId, ReadingTimeSeconds = readingTimeSeconds, ScrollPercentage = scrollPercentage };
                var response = await _httpClient.PostAsJsonAsync("UserDashboard/update-session", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorResult("Okuma oturumu güncellenemedi.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result ?? new ErrorResult("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorResult("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Bir hata oluştu.");
            }
        }

        public async Task<Result> CompleteReadingSessionAsync(Guid postId)
        {
            try
            {
                var request = new { PostId = postId };
                var response = await _httpClient.PostAsJsonAsync("UserDashboard/complete-session", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorResult("Okuma oturumu tamamlanamadı.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result ?? new ErrorResult("Yanıt ayrıştırılamadı.");
            }
            catch (JsonException ex)
            {
                return new ErrorResult("Veri formatı hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Bir hata oluştu.");
            }
        }
    }
}