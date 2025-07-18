using Microsoft.Extensions.Options;

namespace BloggerProUI.Web.Services
{
    public class SeoConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public SeoConfigurationService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public string GetBaseUrl()
        {
            if (_environment.IsDevelopment())
            {
                return "https://localhost:7020";
            }
            
            return _configuration["BaseUrl"] ?? "https://insanlikhallerimiz.com";
        }

        public string GetSitemapUrl()
        {
            return $"{GetBaseUrl()}/sitemap.xml";
        }

        public string GetPostsSitemapUrl()
        {
            return $"{GetBaseUrl()}/sitemap-posts.xml";
        }

        public string GetCanonicalUrl(string path)
        {
            return $"{GetBaseUrl()}{path}";
        }

        public string GetSeoFriendlyUrl(string category, string slug, DateTime? publishDate = null)
        {
            var baseUrl = GetBaseUrl();
            
            // Date-based URL structure: /blog/2024/01/post-slug
            if (publishDate.HasValue)
            {
                return $"{baseUrl}/blog/{publishDate.Value.Year}/{publishDate.Value.Month:D2}/{slug}";
            }
            
            // Category-based URL structure: /blog/category/post-slug
            if (!string.IsNullOrEmpty(category))
            {
                return $"{baseUrl}/blog/{category.ToLower()}/{slug}";
            }
            
            // Default structure: /blog/post-slug
            return $"{baseUrl}/blog/{slug}";
        }

        public string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            // Convert to lowercase and replace Turkish characters
            var slug = title.ToLower()
                .Replace("ç", "c")
                .Replace("ğ", "g")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ş", "s")
                .Replace("ü", "u")
                .Replace("İ", "i")
                .Replace("Ç", "c")
                .Replace("Ğ", "g")
                .Replace("Ö", "o")
                .Replace("Ş", "s")
                .Replace("Ü", "u");

            // Remove special characters and replace spaces with hyphens
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-');

            // Limit length
            if (slug.Length > 60)
            {
                slug = slug.Substring(0, 60).TrimEnd('-');
            }

            return slug;
        }

        public string GetDefaultMetaDescription()
        {
            return "insanlikHallerimiz - Sanat, tasarım ve fotoğrafçılık dünyasından güncel içerikler, ilham verici yazılar ve yaratıcı fikirler.";
        }

        public string GetDefaultKeywords()
        {
            return "blog, sanat, tasarım, fotoğrafçılık, yaratıcılık, insanlik hallerimiz";
        }

        public string GetSiteName()
        {
            return "insanlikHallerimiz";
        }

        public string GetDefaultOgImage()
        {
            return $"{GetBaseUrl()}/images/og-default.jpg";
        }

        public string GetLogoUrl()
        {
            return $"{GetBaseUrl()}/images/logo.png";
        }

        public string GetTwitterHandle()
        {
            return "@insanlikhallerimiz";
        }

        public string OptimizeMetaDescription(string description, int maxLength = 160)
        {
            if (string.IsNullOrEmpty(description))
                return GetDefaultMetaDescription();

            if (description.Length <= maxLength)
                return description;

            // Find the last complete word within the limit
            var truncated = description.Substring(0, maxLength);
            var lastSpace = truncated.LastIndexOf(' ');
            
            if (lastSpace > 0)
            {
                truncated = truncated.Substring(0, lastSpace);
            }

            return truncated + "...";
        }

        public List<string> ExtractKeywords(string content, int maxKeywords = 10)
        {
            if (string.IsNullOrEmpty(content))
                return new List<string>();

            // Simple keyword extraction (can be improved with NLP)
            var words = content.ToLower()
                .Split(new[] { ' ', '\n', '\t', '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}' }, 
                       StringSplitOptions.RemoveEmptyEntries)
                .Where(w => w.Length > 3)
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Take(maxKeywords)
                .Select(g => g.Key)
                .ToList();

            return words;
        }

        public string GenerateRobotsTxt()
        {
            var baseUrl = GetBaseUrl();
            
            return $@"User-agent: *
Allow: /

# Sitemap
Sitemap: {GetSitemapUrl()}
Sitemap: {GetPostsSitemapUrl()}

# Disallow admin areas
Disallow: /Admin/
Disallow: /UserPanel/
Disallow: /api/
Disallow: /Auth/
Disallow: /Account/

# Allow important directories
Allow: /Blog/
Allow: /css/
Allow: /js/
Allow: /images/
Allow: /uploads/

# Performance optimizations
Crawl-delay: 1

# Common SEO directives
User-agent: Googlebot
Crawl-delay: 0

User-agent: Bingbot
Crawl-delay: 1";
        }
    }
}