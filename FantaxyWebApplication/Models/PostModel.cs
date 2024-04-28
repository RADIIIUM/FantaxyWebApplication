using System.Diagnostics;
using FantaxyWebApplication.Models.Entities;

namespace FantaxyWebApplication.Models
{
    public class PostModel
    {
        public int IdPost { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; } = "Новый пост";

        public PlanetUsersInfo authorInfo { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public List<string>? Files { get; set; }
        public bool IsPined { get; set; } = false;
    }
}
