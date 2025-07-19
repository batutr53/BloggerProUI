using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Chat;
using BloggerProUI.Shared.Utilities.Results;
using System.Text;
using System.Text.Json;

namespace BloggerProUI.Business.Services
{
    public class PresenceApiService : IPresenceApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public PresenceApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<Result> UpdatePresenceAsync(bool isOnline)
        {
            try
            {
                var requestDto = new { IsOnline = isOnline };
                var json = JsonSerializer.Serialize(requestDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("presence/update", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Result>(responseContent, _jsonOptions);
                    return result ?? new ErrorResult("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorResult>(responseContent, _jsonOptions);
                    return errorResult ?? new ErrorResult("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<UserPresenceDto>> GetUserPresenceAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"presence/{userId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<UserPresenceDto>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<UserPresenceDto>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<UserPresenceDto>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<UserPresenceDto>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserPresenceDto>($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<List<UserPresenceDto>>> GetMultipleUserPresenceAsync(List<Guid> userIds)
        {
            try
            {
                var json = JsonSerializer.Serialize(userIds, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("presence/multiple", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<List<UserPresenceDto>>>(responseContent, _jsonOptions);
                    return result ?? new ErrorDataResult<List<UserPresenceDto>>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<List<UserPresenceDto>>>(responseContent, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<List<UserPresenceDto>>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserPresenceDto>>($"Error: {ex.Message}");
            }
        }
    }
}