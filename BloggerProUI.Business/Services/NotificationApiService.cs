using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Notification;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services
{
    public class NotificationApiService : INotificationApiService
    {
        private readonly HttpClient _httpClient;

        public NotificationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDataResult<List<NotificationDto>>> GetUserNotificationsAsync(bool onlyUnread = false, int page = 1, int pageSize = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"notification?onlyUnread={onlyUnread}&page={page}&pageSize={pageSize}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<List<NotificationDto>>("Bildirimler alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<List<NotificationDto>>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<NotificationDto>>>();
                return result ?? new ErrorDataResult<List<NotificationDto>>("Veri deserialize edilemedi.");
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<List<NotificationDto>>("Sunucuya bağlanırken bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<NotificationDto>>($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IDataResult<NotificationDto>> GetNotificationByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"notification/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<NotificationDto>("Bildirim bulunamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<NotificationDto>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<NotificationDto>>();
                return result ?? new ErrorDataResult<NotificationDto>("Veri deserialize edilemedi.");
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<NotificationDto>("Sunucuya bağlanırken bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<NotificationDto>($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IDataResult<int>> GetUnreadCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("notification/unread-count");

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<int>("Okunmamış bildirim sayısı alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<int>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<int>>();
                return result ?? new ErrorDataResult<int>("Veri deserialize edilemedi.");
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<int>("Sunucuya bağlanırken bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<int>($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IResult> MarkAsReadAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"notification/{id}/read", null);

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorResult("Bildirim okundu olarak işaretlenemedi.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorResult("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result ?? new ErrorResult("Veri deserialize edilemedi.");
            }
            catch (HttpRequestException)
            {
                return new ErrorResult("Sunucuya bağlanırken bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IResult> MarkAllAsReadAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("notification/mark-all-read", null);

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorResult("Tüm bildirimler okundu olarak işaretlenemedi.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorResult("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result ?? new ErrorResult("Veri deserialize edilemedi.");
            }
            catch (HttpRequestException)
            {
                return new ErrorResult("Sunucuya bağlanırken bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IDataResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("notification", notificationDto);

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<NotificationDto>("Bildirim oluşturulamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<NotificationDto>("Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<NotificationDto>>();
                return result ?? new ErrorDataResult<NotificationDto>("Veri deserialize edilemedi.");
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<NotificationDto>("Sunucuya bağlanırken bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<NotificationDto>($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }
    }
}