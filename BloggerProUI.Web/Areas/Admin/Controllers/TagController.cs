using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TagController : Controller
    {
        private readonly ITagApiService _tagApiService;

        public TagController(ITagApiService tagApiService)
        {
            _tagApiService = tagApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _tagApiService.GetAllAsync();
            if (!result.Success)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Message);
                return View(new List<TagDto>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TagCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _tagApiService.CreateAsync(dto);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Message);
                return View(dto);
            }

            TempData["SuccessMessage"] = string.Join(" ", result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _tagApiService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _tagApiService.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Message);
                return View(dto);
            }

            TempData["SuccessMessage"] = string.Join(" ", result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _tagApiService.DeleteAsync(id);
            return Json(new
            {
                success = result.Success,
                message = string.Join(" ", result.Message)
            });
        }
    }
}
