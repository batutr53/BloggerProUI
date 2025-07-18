using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using BloggerProUI.Business.Interfaces;

namespace BloggerProUI.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IPostApiService _postApiService;
        private readonly ICategoryApiService _categoryApiService;
        private readonly ITagApiService _tagApiService;

        public SitemapController(IPostApiService postApiService, ICategoryApiService categoryApiService, ITagApiService tagApiService)
        {
            _postApiService = postApiService;
            _categoryApiService = categoryApiService;
            _tagApiService = tagApiService;
        }

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Homepage
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/</loc>");
            sb.AppendLine($"<lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("<changefreq>daily</changefreq>");
            sb.AppendLine("<priority>1.0</priority>");
            sb.AppendLine("</url>");

            // Blog main page
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/Blog</loc>");
            sb.AppendLine($"<lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("<changefreq>daily</changefreq>");
            sb.AppendLine("<priority>0.9</priority>");
            sb.AppendLine("</url>");

            // About page
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/Home/About</loc>");
            sb.AppendLine($"<lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("<changefreq>monthly</changefreq>");
            sb.AppendLine("<priority>0.7</priority>");
            sb.AppendLine("</url>");

            // Contact page
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/Home/Contact</loc>");
            sb.AppendLine($"<lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("<changefreq>monthly</changefreq>");
            sb.AppendLine("<priority>0.6</priority>");
            sb.AppendLine("</url>");

            try
            {
                // Get all posts
                var postsResponse = await _postApiService.GetAllPostsAsync(1, 1000);
                if (postsResponse.Success && postsResponse.Data?.Items != null)
                {
                    foreach (var post in postsResponse.Data.Items)
                    {
                        sb.AppendLine("<url>");
                        sb.AppendLine($"<loc>{baseUrl}/Blog/{post.Slug}</loc>");
                        sb.AppendLine($"<lastmod>{(post.LastModified ?? post.CreatedAt):yyyy-MM-dd}</lastmod>");
                        sb.AppendLine("<changefreq>weekly</changefreq>");
                        sb.AppendLine("<priority>0.8</priority>");
                        sb.AppendLine("</url>");
                    }
                }

                // Get all categories
                var categoriesResponse = await _categoryApiService.GetAllAsync();
                if (categoriesResponse.Success && categoriesResponse.Data != null)
                {
                    foreach (var category in categoriesResponse.Data)
                    {
                        sb.AppendLine("<url>");
                        sb.AppendLine($"<loc>{baseUrl}/Blog?category={Uri.EscapeDataString(category.Slug)}</loc>");
                        sb.AppendLine($"<lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
                        sb.AppendLine("<changefreq>weekly</changefreq>");
                        sb.AppendLine("<priority>0.7</priority>");
                        sb.AppendLine("</url>");
                    }
                }

                // Get all tags
                var tagsResponse = await _tagApiService.GetAllAsync();
                if (tagsResponse.Success && tagsResponse.Data != null)
                {
                    foreach (var tag in tagsResponse.Data.Take(50)) // Limit to top 50 tags
                    {
                        sb.AppendLine("<url>");
                        sb.AppendLine($"<loc>{baseUrl}/Blog?tag={Uri.EscapeDataString(tag.Name)}</loc>");
                        sb.AppendLine($"<lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
                        sb.AppendLine("<changefreq>weekly</changefreq>");
                        sb.AppendLine("<priority>0.6</priority>");
                        sb.AppendLine("</url>");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with basic sitemap
                Console.WriteLine($"Error generating sitemap: {ex.Message}");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }

        [Route("sitemap-posts.xml")]
        public async Task<IActionResult> PostsSitemap()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">");

            try
            {
                // Get all posts
                var postsResponse = await _postApiService.GetAllPostsAsync(1, 1000);
                if (postsResponse.Success && postsResponse.Data?.Items != null)
                {
                    foreach (var post in postsResponse.Data.Items)
                    {
                        sb.AppendLine("<url>");
                        sb.AppendLine($"<loc>{baseUrl}/Blog/{post.Slug}</loc>");
                        sb.AppendLine($"<lastmod>{(post.LastModified ?? post.CreatedAt):yyyy-MM-dd}</lastmod>");
                        sb.AppendLine("<changefreq>weekly</changefreq>");
                        sb.AppendLine("<priority>0.8</priority>");
                        
                        // Add news sitemap data for recent posts
                        if (post.CreatedAt > DateTime.Now.AddDays(-30))
                        {
                            sb.AppendLine("<news:news>");
                            sb.AppendLine("<news:publication>");
                            sb.AppendLine("<news:name>insanlikHallerimiz</news:name>");
                            sb.AppendLine("<news:language>tr</news:language>");
                            sb.AppendLine("</news:publication>");
                            sb.AppendLine("<news:publication_date>" + post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss+00:00") + "</news:publication_date>");
                            sb.AppendLine($"<news:title>{XmlEscape(post.Title)}</news:title>");
                            sb.AppendLine("</news:news>");
                        }
                        
                        sb.AppendLine("</url>");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating posts sitemap: {ex.Message}");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }

        private string XmlEscape(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Replace("&", "&amp;")
                      .Replace("<", "&lt;")
                      .Replace(">", "&gt;")
                      .Replace("\"", "&quot;")
                      .Replace("'", "&#39;");
        }
    }
}