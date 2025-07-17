using BloggerProUI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Controllers;

public class FooterController : Controller
{
    private readonly IFooterApiService _footerApiService;

    public FooterController(IFooterApiService footerApiService)
    {
        _footerApiService = footerApiService;
    }

    public async Task<IActionResult> GetActiveFooters()
    {
        var result = await _footerApiService.GetActiveFootersAsync();
        
        if (result.Success)
        {
            return Json(result.Data);
        }
        
        return Json(new List<object>());
    }

    public async Task<IActionResult> GetFootersByType(string footerType)
    {
        var result = await _footerApiService.GetFootersByTypeAsync(footerType);
        
        if (result.Success)
        {
            return Json(result.Data);
        }
        
        return Json(new List<object>());
    }

    public async Task<IActionResult> RenderFooterContent()
    {
        var result = await _footerApiService.GetActiveFootersAsync();
        
        if (result.Success && result.Data.Any())
        {
            return PartialView("_DynamicFooter", result.Data);
        }
        
        return PartialView("_DefaultFooter");
    }

    public async Task<IActionResult> GetFooterSection(string sectionType)
    {
        var result = await _footerApiService.GetFootersByTypeAsync(sectionType);
        
        if (result.Success)
        {
            return PartialView("_FooterSection", result.Data);
        }
        
        return PartialView("_EmptyFooterSection");
    }
}