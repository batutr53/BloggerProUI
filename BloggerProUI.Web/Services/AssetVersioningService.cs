using BloggerProUI.Web.Models.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace BloggerProUI.Web.Services
{
    public interface IAssetVersioningService
    {
        string GetVersionedUrl(string assetPath);
        string GetCssVersion();
        string GetJsVersion();
        void UpdateCssVersion();
        void UpdateJsVersion();
    }

    public class AssetVersioningService : IAssetVersioningService
    {
        private readonly AssetVersioningOptions _options;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AssetVersioningService> _logger;
        private readonly IConfiguration _configuration;

        public AssetVersioningService(
            IOptions<AssetVersioningOptions> options,
            IWebHostEnvironment environment,
            ILogger<AssetVersioningService> logger,
            IConfiguration configuration)
        {
            _options = options.Value;
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        public string GetVersionedUrl(string assetPath)
        {
            if (!_options.EnableVersioning)
                return assetPath;

            try
            {
                if (_options.UseFileHash)
                {
                    var version = GetFileHashVersion(assetPath);
                    return $"{assetPath}?v={version}";
                }
                else
                {
                    var version = assetPath.Contains(".css") ? _options.CssVersion : _options.JsVersion;
                    return $"{assetPath}?v={version}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating versioned URL for asset: {AssetPath}", assetPath);
                return $"{assetPath}?v={DateTime.Now.Ticks}";
            }
        }

        public string GetCssVersion()
        {
            return _options.CssVersion;
        }

        public string GetJsVersion()
        {
            return _options.JsVersion;
        }

        public void UpdateCssVersion()
        {
            var version = IncrementVersion(_options.CssVersion);
            UpdateVersionInConfig("AssetVersioning:CssVersion", version);
            _logger.LogInformation("CSS version updated to: {Version}", version);
        }

        public void UpdateJsVersion()
        {
            var version = IncrementVersion(_options.JsVersion);
            UpdateVersionInConfig("AssetVersioning:JsVersion", version);
            _logger.LogInformation("JS version updated to: {Version}", version);
        }

        private string GetFileHashVersion(string assetPath)
        {
            var filePath = Path.Combine(_environment.WebRootPath, assetPath.TrimStart('/'));
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Asset file not found: {FilePath}", filePath);
                return DateTime.Now.Ticks.ToString();
            }

            var fileBytes = File.ReadAllBytes(filePath);
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(fileBytes);
            return Convert.ToHexString(hash)[..8].ToLower(); // İlk 8 karakter
        }

        private string IncrementVersion(string version)
        {
            try
            {
                var parts = version.Split('.');
                if (parts.Length == 3)
                {
                    var major = int.Parse(parts[0]);
                    var minor = int.Parse(parts[1]);
                    var patch = int.Parse(parts[2]);
                    
                    patch++;
                    if (patch >= 100)
                    {
                        patch = 0;
                        minor++;
                        if (minor >= 100)
                        {
                            minor = 0;
                            major++;
                        }
                    }
                    
                    return $"{major}.{minor}.{patch}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing version: {Version}", version);
            }
            
            // Fallback to timestamp
            return DateTime.Now.ToString("yyyy.MM.dd");
        }

        private void UpdateVersionInConfig(string key, string value)
        {
            try
            {
                var configPath = Path.Combine(_environment.ContentRootPath, "appsettings.json");
                var json = File.ReadAllText(configPath);
                var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                
                if (config != null)
                {
                    if (config.ContainsKey("AssetVersioning"))
                    {
                        var assetVersioning = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(config["AssetVersioning"].ToString());
                        if (assetVersioning != null)
                        {
                            var keyPart = key.Split(':').Last();
                            assetVersioning[keyPart] = value;
                            config["AssetVersioning"] = assetVersioning;
                        }
                    }
                    
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    
                    var updatedJson = System.Text.Json.JsonSerializer.Serialize(config, options);
                    File.WriteAllText(configPath, updatedJson);
                    
                    _logger.LogInformation("Configuration updated: {Key} = {Value}", key, value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating configuration: {Key}", key);
            }
        }
    }
}