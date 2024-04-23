using System.Diagnostics;

namespace FantaxyWebApplication.Models
{
    public class PostModel
    {
        public int IdPost { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; } = "Новый пост";

        public string? AuthorLogin { get; set; }
        public string? AuthorImagePath { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public bool IsPined { get; set; } = false;
    }
}
