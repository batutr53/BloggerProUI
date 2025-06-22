﻿using BloggerProUI.Models.PostModule;

namespace BloggerProUI.Models.Post
{
    public class PostCreateDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string FeaturedImage { get; set; } = null!;

        public List<Guid> CategoryIds { get; set; } = new();
        public List<Guid> TagIds { get; set; } = new();
        public List<PostModuleDto> Modules { get; set; } = new();
    }

}
