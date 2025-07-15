using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BloggerProUI.Business.Interfaces
{
    public interface IImageApiService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}