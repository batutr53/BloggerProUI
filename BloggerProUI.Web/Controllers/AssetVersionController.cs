using BloggerProUI.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetVersionController : ControllerBase
    {
        private readonly IAssetVersioningService _assetVersioningService;
        private readonly ILogger<AssetVersionController> _logger;

        public AssetVersionController(
            IAssetVersioningService assetVersioningService,
            ILogger<AssetVersionController> logger)
        {
            _assetVersioningService = assetVersioningService;
            _logger = logger;
        }

        [HttpGet("versions")]
        public IActionResult GetVersions()
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        cssVersion = _assetVersioningService.GetCssVersion(),
                        jsVersion = _assetVersioningService.GetJsVersion(),
                        timestamp = DateTime.Now
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset versions");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("update-css")]
        public IActionResult UpdateCssVersion()
        {
            try
            {
                _assetVersioningService.UpdateCssVersion();
                return Ok(new
                {
                    success = true,
                    message = "CSS version updated successfully",
                    newVersion = _assetVersioningService.GetCssVersion()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating CSS version");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("update-js")]
        public IActionResult UpdateJsVersion()
        {
            try
            {
                _assetVersioningService.UpdateJsVersion();
                return Ok(new
                {
                    success = true,
                    message = "JS version updated successfully",
                    newVersion = _assetVersioningService.GetJsVersion()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating JS version");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("update-all")]
        public IActionResult UpdateAllVersions()
        {
            try
            {
                _assetVersioningService.UpdateCssVersion();
                _assetVersioningService.UpdateJsVersion();
                
                return Ok(new
                {
                    success = true,
                    message = "All versions updated successfully",
                    versions = new
                    {
                        cssVersion = _assetVersioningService.GetCssVersion(),
                        jsVersion = _assetVersioningService.GetJsVersion()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating all versions");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}