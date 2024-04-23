using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PostsInfo
    {
        public int IdPost { get; set; }
        public string? Title { get; set; }
        public string PostText { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Background { get; set; }

        public virtual Post IdPostNavigation { get; set; } = null!;
    }
}
