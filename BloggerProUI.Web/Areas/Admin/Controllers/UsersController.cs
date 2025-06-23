using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserApiService _userApiService;

        public UsersController(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userApiService.GetAllUsersWithRolesAsync();
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message?.FirstOrDefault();
                return View(new List<UserListDto>());
            }

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles([FromBody] UpdateUserRolesDto dto)
        {
            var result = await _userApiService.UpdateUserRolesAsync(dto);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleBlock(Guid userId, bool block)
        {
            var result = await _userApiService.ToggleUserBlockAsync(new ToggleUserBlockDto
            {
                UserId = userId,
                Block = block
            });

            return Json(result);
        }
    }

}
