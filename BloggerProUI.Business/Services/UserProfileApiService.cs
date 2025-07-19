using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BloggerProUI.Business.Services
{
    public class UserProfileApiService : IUserProfileApiService
    {
        private readonly HttpClient _httpClient;

        public UserProfileApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResult<UserProfileDto>> GetCurrentUserProfileAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/user-profile");

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<UserProfileDto>("Profil bilgileri alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new ErrorDataResult<UserProfileDto>("Boş yanıt alındı.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<DataResult<UserProfileDto>>();
                return apiResult ?? new ErrorDataResult<UserProfileDto>("Profil verisi çözümlenemedi.");
            }
            catch (JsonException)
            {
                return new ErrorDataResult<UserProfileDto>("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserProfileDto>($"Profil bilgileri alınırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<DataResult<UserProfileDto>> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/user-profile/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<UserProfileDto>("Kullanıcı profili bulunamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new ErrorDataResult<UserProfileDto>("Boş yanıt alındı.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<DataResult<UserProfileDto>>();
                return apiResult ?? new ErrorDataResult<UserProfileDto>("Profil verisi çözümlenemedi.");
            }
            catch (JsonException)
            {
                return new ErrorDataResult<UserProfileDto>("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserProfileDto>($"Kullanıcı profili alınırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> UpdateProfileAsync(UpdateUserProfileDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("/api/user-profile", dto);

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorResult("Profil güncellenemedi.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new SuccessResult("Profil başarıyla güncellendi.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<Result>();
                return apiResult ?? new SuccessResult("Profil başarıyla güncellendi.");
            }
            catch (JsonException)
            {
                return new ErrorResult("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Profil güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordDto dto)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync("/api/user-profile/change-password", dto);

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorResult("Şifre değiştirilemedi.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new SuccessResult("Şifre başarıyla değiştirildi.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<Result>();
                return apiResult ?? new SuccessResult("Şifre başarıyla değiştirildi.");
            }
            catch (JsonException)
            {
                return new ErrorResult("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Şifre değiştirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<DataResult<string>> UploadProfileImageAsync(IFormFile file)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", file.FileName);

                var response = await _httpClient.PostAsync("/api/user-profile/upload-avatar", content);

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<string>("Profil resmi yüklenemedi.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return new ErrorDataResult<string>("Boş yanıt alındı.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<DataResult<string>>();
                return apiResult ?? new ErrorDataResult<string>("Profil resmi verisi çözümlenemedi.");
            }
            catch (JsonException)
            {
                return new ErrorDataResult<string>("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>($"Profil resmi yüklenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> FollowUserAsync(Guid userId)
        {
            try
            {
                // Note: Follow functionality may need a separate controller/endpoint in BloggerPro API
                var response = await _httpClient.PostAsJsonAsync("/api/user-follower", new { FollowingUserId = userId });

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorResult("Kullanıcı takip edilemedi.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new SuccessResult("Kullanıcı başarıyla takip edildi.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<Result>();
                return apiResult ?? new SuccessResult("Kullanıcı başarıyla takip edildi.");
            }
            catch (JsonException)
            {
                return new ErrorResult("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Kullanıcı takip edilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> UnfollowUserAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/user-follower/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorResult("Kullanıcı takipten çıkarılamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new SuccessResult("Kullanıcı takipten başarıyla çıkarıldı.");
                }

                var apiResult = await response.Content.ReadFromJsonAsync<Result>();
                return apiResult ?? new SuccessResult("Kullanıcı takipten başarıyla çıkarıldı.");
            }
            catch (JsonException)
            {
                return new ErrorResult("JSON çözümleme hatası.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Kullanıcı takipten çıkarılırken hata oluştu: {ex.Message}");
            }
        }
    }
}