using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IDataResult<List<UserListDto>>> GetAllUsersWithRolesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("UserAdminModeration/all-users");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<DataResult<List<UserListDto>>>(
                        await response.Content.ReadAsStringAsync());

                    if (result != null)
                        return result;
                }

                Console.Error.WriteLine($"{DateTime.Now:yyyy.MM.dd:HH:mm:ss} | {response.ReasonPhrase}");

                var errorResult = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                if (errorResult == null || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new ErrorDataResult<List<UserListDto>>("Beklenmeyen bir hata oluştu.");
                }

                return new ErrorDataResult<List<UserListDto>>(errorResult.Message, errorResult.HttpStatusCode, errorResult.StatusCode);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{DateTime.Now:yyyy.MM.dd:HH:mm:ss} | {ex.Message}");
                return new ErrorDataResult<List<UserListDto>>(ex.Message);
            }
        }

        public async Task<IResult> UpdateUserRolesAsync(UpdateUserRolesDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("UserAdminModeration/update-roles", dto);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorResult("Sonuç null döndü.");
                }

                Console.Error.WriteLine($"{DateTime.Now:yyyy.MM.dd:HH:mm:ss} | {response.ReasonPhrase}");

                var errorResult = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorResult("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{DateTime.Now:yyyy.MM.dd:HH:mm:ss} | {ex.Message}");
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IResult> ToggleUserBlockAsync(ToggleUserBlockDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("UserAdminModeration/toggle-block", dto);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorResult("Sonuç null döndü.");
                }

                Console.Error.WriteLine($"{DateTime.Now:yyyy.MM.dd:HH:mm:ss} | {response.ReasonPhrase}");

                var errorResult = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorResult("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{DateTime.Now:yyyy.MM.dd:HH:mm:ss} | {ex.Message}");
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IResult> FollowUserAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"UserFollower/follow/{userId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorResult("Sonuç null döndü.");
                }

                var errorResult = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorResult("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IResult> UnfollowUserAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"UserFollower/unfollow/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorResult("Sonuç null döndü.");
                }

                var errorResult = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorResult("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IDataResult<bool>> IsFollowingAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserFollower/is-following/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<DataResult<bool>>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorDataResult<bool>("Sonuç null döndü.");
                }

                var errorResult = JsonConvert.DeserializeObject<ErrorDataResult<bool>>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorDataResult<bool>("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<bool>(ex.Message);
            }
        }

        public async Task<IDataResult<List<UserDto>>> GetFollowersAsync(Guid userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserFollower/followers/{userId}?page={page}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<DataResult<List<UserDto>>>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorDataResult<List<UserDto>>("Sonuç null döndü.");
                }

                var errorResult = JsonConvert.DeserializeObject<ErrorDataResult<List<UserDto>>>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorDataResult<List<UserDto>>("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserDto>>(ex.Message);
            }
        }

        public async Task<IDataResult<List<UserDto>>> GetFollowingAsync(Guid userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserFollower/following/{userId}?page={page}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<DataResult<List<UserDto>>>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorDataResult<List<UserDto>>("Sonuç null döndü.");
                }

                var errorResult = JsonConvert.DeserializeObject<ErrorDataResult<List<UserDto>>>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorDataResult<List<UserDto>>("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserDto>>(ex.Message);
            }
        }

        public async Task<IDataResult<List<UserRecommendationDto>>> GetRecommendationsAsync(int limit = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"UserFollower/recommendations?limit={limit}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<DataResult<List<UserRecommendationDto>>>(await response.Content.ReadAsStringAsync());
                    return result ?? new ErrorDataResult<List<UserRecommendationDto>>("Sonuç null döndü.");
                }

                var errorResult = JsonConvert.DeserializeObject<ErrorDataResult<List<UserRecommendationDto>>>(await response.Content.ReadAsStringAsync());
                return errorResult ?? new ErrorDataResult<List<UserRecommendationDto>>("Hata sonucu çözümlenemedi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserRecommendationDto>>(ex.Message);
            }
        }
    }

}
