using BloggerProUI.Models.Enums;
using BloggerProUI.Models.SeoMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggerProUI.Models.PostModule
{
    public class PostModuleDto
    {
        public Guid Id { get; set; }
        public PostModuleType Type { get; set; }
        public string? Content { get; set; }
        public string? MediaUrl { get; set; }
        public string? Alignment { get; set; }
        public string? Width { get; set; }
        public int Order { get; set; }
        public int SortOrder { get; set; }
        public List<SeoMetadataDto> SeoMetadata { get; set; } = new();

    }
}
