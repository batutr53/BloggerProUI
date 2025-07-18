using BloggerProUI.Web.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BloggerProUI.Web.TagHelpers
{
    [HtmlTargetElement("link", Attributes = "asp-versioned")]
    [HtmlTargetElement("script", Attributes = "asp-versioned")]
    public class VersionedAssetTagHelper : TagHelper
    {
        private readonly IAssetVersioningService _assetVersioningService;

        public VersionedAssetTagHelper(IAssetVersioningService assetVersioningService)
        {
            _assetVersioningService = assetVersioningService;
        }

        [HtmlAttributeName("asp-versioned")]
        public bool EnableVersioning { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!EnableVersioning)
                return;

            // Link tag için href attribute'u kontrol et
            if (output.TagName == "link" && output.Attributes.ContainsName("href"))
            {
                var href = output.Attributes["href"].Value?.ToString();
                if (!string.IsNullOrEmpty(href) && !href.StartsWith("http"))
                {
                    var versionedUrl = _assetVersioningService.GetVersionedUrl(href);
                    output.Attributes.SetAttribute("href", versionedUrl);
                }
            }

            // Script tag için src attribute'u kontrol et
            if (output.TagName == "script" && output.Attributes.ContainsName("src"))
            {
                var src = output.Attributes["src"].Value?.ToString();
                if (!string.IsNullOrEmpty(src) && !src.StartsWith("http"))
                {
                    var versionedUrl = _assetVersioningService.GetVersionedUrl(src);
                    output.Attributes.SetAttribute("src", versionedUrl);
                }
            }
        }
    }
}