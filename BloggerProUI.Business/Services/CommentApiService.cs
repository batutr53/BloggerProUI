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

        public async Task<DataResult<List<CommentListDto>>> GetMostLikedCommentsAsync(int count = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Comment/most-liked?count={count}");
                
                if (!response.IsSuccessStatusCode)
                {
                    return new DataResult<List<CommentListDto>>
                    {
                        Success = false,
                        Message = new[] { "En çok beğenilen yorumlar alınırken bir hata oluştu." },
                        Data = new List<CommentListDto>()
                    };
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new DataResult<List<CommentListDto>>
                    {
                        Success = false,
                        Message = new[] { "Boş yanıt alındı." },
                        Data = new List<CommentListDto>()
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<CommentListDto>>>();
                return result ?? new DataResult<List<CommentListDto>>
                {
                    Success = false,
                    Message = new[] { "Veri işlenirken hata oluştu." },
                    Data = new List<CommentListDto>()
                };
            }
            catch (Exception ex)
            {
                return new DataResult<List<CommentListDto>>
                {
                    Success = false,
                    Message = new[] { $"Bir hata oluştu: {ex.Message}" },
                    Data = new List<CommentListDto>()
                };
            }
        }

        public async Task<DataResult<List<RecentCommentDto>>> GetRecentCommentsAsync(int count = 5)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Comment/recent?count={count}");

                if (!response.IsSuccessStatusCode)
                {
                    return new DataResult<List<RecentCommentDto>>
                    {
                        Success = false,
                        Message = new[] { "Son yorumlar alınırken bir hata oluştu." },
                        Data = new List<RecentCommentDto>()
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<RecentCommentDto>>>();
                return result ?? new DataResult<List<RecentCommentDto>>
                {
                    Success = false,
                    Message = new[] { "Veri işlenirken hata oluştu." },
                    Data = new List<RecentCommentDto>()
                };
            }
            catch (Exception ex)
            {
                return new DataResult<List<RecentCommentDto>>
                {
                    Success = false,
                    Message = new[] { $"Bir hata oluştu: {ex.Message}" },
                    Data = new List<RecentCommentDto>()
                };
            }
        }
    }

}
