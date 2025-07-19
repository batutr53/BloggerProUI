using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.User;
using BloggerProUI.Shared.Utilities.Results;
using System.Text.Json;

namespace BloggerProUI.Business.Services
{
    public class UserSearchApiService : IUserSearchApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserSearchApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<DataResult<List<UserSearchDto>>> SearchUsersAsync(string query, bool includeMutual = true, int limit = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"usersearch/search?q={Uri.EscapeDataString(query)}&mutual={includeMutual}&limit={limit}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<List<UserSearchDto>>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<List<UserSearchDto>>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<List<UserSearchDto>>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<List<UserSearchDto>>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserSearchDto>>($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<List<UserRecommendationDto>>> GetRecommendationsAsync(int limit = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"usersearch/recommendations?limit={limit}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<List<UserRecommendationDto>>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<List<UserRecommendationDto>>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<List<UserRecommendationDto>>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<List<UserRecommendationDto>>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserRecommendationDto>>($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<List<UserDto>>> GetMutualConnectionsAsync(Guid userId, Guid otherUserId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"usersearch/mutuals/{userId}/{otherUserId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<List<UserDto>>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<List<UserDto>>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<List<UserDto>>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<List<UserDto>>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserDto>>($"Error: {ex.Message}");
            }
        }
    }
}