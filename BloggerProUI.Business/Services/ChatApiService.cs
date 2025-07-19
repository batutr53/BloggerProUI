using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Chat;
using BloggerProUI.Shared.Utilities.Results;
using System.Text;
using System.Text.Json;

namespace BloggerProUI.Business.Services
{
    public class ChatApiService : IChatApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ChatApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<DataResult<List<ConversationDto>>> GetConversationsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"chat/conversations?page={page}&pageSize={pageSize}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<List<ConversationDto>>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<List<ConversationDto>>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<List<ConversationDto>>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<List<ConversationDto>>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<ConversationDto>>($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<List<MessageDto>>> GetMessagesAsync(Guid otherUserId, int page = 1, int pageSize = 50)
        {
            try
            {
                var response = await _httpClient.GetAsync($"chat/messages/{otherUserId}?page={page}&pageSize={pageSize}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<List<MessageDto>>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<List<MessageDto>>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<List<MessageDto>>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<List<MessageDto>>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<MessageDto>>($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<MessageDto>> SendMessageAsync(CreateMessageDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("chat/send", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<MessageDto>>(responseContent, _jsonOptions);
                    return result ?? new ErrorDataResult<MessageDto>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<MessageDto>>(responseContent, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<MessageDto>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<MessageDto>($"Error: {ex.Message}");
            }
        }

        public async Task<Result> MarkMessagesAsReadAsync(MarkMessagesAsReadDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("chat/mark-read", content);
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

        public async Task<DataResult<bool>> CanUsersStartConversationAsync(Guid userId1, Guid userId2)
        {
            try
            {
                var response = await _httpClient.GetAsync($"chat/can-chat/{userId1}/{userId2}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<bool>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<bool>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<bool>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<bool>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<bool>($"Error: {ex.Message}");
            }
        }

        public async Task<Result> MarkMessagesAsDeliveredAsync(MarkMessagesAsDeliveredDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("chat/mark-delivered", content);
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

        public async Task<DataResult<int>> GetUnreadMessageCountAsync(Guid senderId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"chat/unread-count/{senderId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<int>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<int>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<int>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<int>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<int>($"Error: {ex.Message}");
            }
        }

        public async Task<DataResult<int>> GetTotalUnreadCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("chat/total-unread-count");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<DataResult<int>>(content, _jsonOptions);
                    return result ?? new ErrorDataResult<int>("Deserialization failed");
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ErrorDataResult<int>>(content, _jsonOptions);
                    return errorResult ?? new ErrorDataResult<int>("API call failed");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<int>($"Error: {ex.Message}");
            }
        }
    }
}