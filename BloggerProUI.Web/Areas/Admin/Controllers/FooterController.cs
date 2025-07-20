using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Footer;
using BloggerProUI.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class FooterController : Controller
{
    private readonly IFooterApiService _footerApiService;

    public FooterController(IFooterApiService footerApiService)
    {
        _footerApiService = footerApiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _footerApiService.GetAllFootersAsync();
        
        if (result.Success)
        {
            return View(result.Data);
        }
        
        TempData["ErrorMessage"] = result.Message;
        return View(new List<FooterDto>());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FooterCreateDto model)
    {   
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _footerApiService.CreateFooterAsync(model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Footer başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // Detaylı hata mesajını göster
        TempData["ErrorMessage"] = $"Footer oluşturulurken hata: {result.Message}";
        
        // Model state'e de hata ekle
        
        return View(model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _footerApiService.GetFooterByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var updateModel = new FooterUpdateDto
        {
            Id = result.Data.Id,
            SectionTitle = result.Data.SectionTitle,
            Content = result.Data.Content,
            LinkUrl = result.Data.LinkUrl,
            LinkText = result.Data.LinkText,
            SortOrder = result.Data.SortOrder,
            IsActive = result.Data.IsActive,
            FooterType = result.Data.FooterType,
            IconClass = result.Data.IconClass,
            ParentSection = result.Data.ParentSection
        };

        return View(updateModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(FooterUpdateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _footerApiService.UpdateFooterAsync(model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Footer başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result.Message;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _footerApiService.DeleteFooterAsync(id);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Footer başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _footerApiService.ToggleFooterStatusAsync(id);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Footer durumu başarıyla güncellendi.";
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}