using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Comment;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services
{
    public class CommentApiService : ICommentApiService
    {
        private readonly HttpClient _httpClient;

        public CommentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResult<List<CommentListDto>>> GetCommentsByPostAsync(Guid postId)
        {
            return await _httpClient.GetFromJsonAsync<DataResult<List<CommentListDto>>>($"/api/Comment/post/{postId}");
        }

        public async Task<DataResult<Guid>> AddCommentAsync(CommentCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Comment", dto);
            return await response.Content.ReadFromJsonAsync<DataResult<Guid>>();
        }

        public async Task<Result> DeleteCommentAsync(Guid commentId)
        {
            var response = await _httpClient.DeleteAsync($"/api/Comment/{commentId}");
            return await response.Content.ReadFromJsonAsync<Result>();
        }
    }

}
