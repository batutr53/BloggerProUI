namespace BloggerProUI.Web.Models.Configuration
{
    public class AssetVersioningOptions
    {
        public const string SectionName = "AssetVersioning";

        public string CssVersion { get; set; } = "1.0.0";
        public string JsVersion { get; set; } = "1.0.0";
        public bool EnableVersioning { get; set; } = true;
        public bool UseFileHash { get; set; } = true;
    }
}