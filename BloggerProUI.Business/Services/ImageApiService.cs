using BloggerProUI.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BloggerProUI.Business.Services
{
    public class ImageApiService : IImageApiService
    {
        private readonly HttpClient _httpClient;

        public ImageApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using (var content = new MultipartFormDataContent())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileContent = new ByteArrayContent(memoryStream.ToArray());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, "upload", file.FileName);

                    var response = await _httpClient.PostAsync("/api/Image/upload", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}