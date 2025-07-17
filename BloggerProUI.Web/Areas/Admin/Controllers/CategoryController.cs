using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryApiService _categoryApiService;

    public CategoryController(ICategoryApiService categoryApiService)
    {
        _categoryApiService = categoryApiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _categoryApiService.GetAllAsync();
        return View(result.Data);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var result = await _categoryApiService.CreateAsync(dto);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = string.Join(", ", result.Message);
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _categoryApiService.GetByIdAsync(id);
        if (!result.Success) return NotFound();

        var dto = new CategoryUpdateDto { Id = result.Data.Id, Name = result.Data.Name };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryUpdateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var result = await _categoryApiService.UpdateAsync(dto);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = string.Join(", ", result.Message);
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryApiService.DeleteAsync(id);
        if (!result.Success)
            return Json(new { success = false, message = result.Message });

        return Json(new { success = true, message = "Kategori başarıyla silindi." });
    }
}
