using BloggerProUI.Models.Auth;
using BloggerProUI.Shared.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggerProUI.Business.Interfaces
{
    public interface IAuthApiService
    {
        Task<IDataResult<string>> LoginAsync(LoginDto dto);
        Task<IResult> RegisterAsync(RegisterDto dto);
    }
}
