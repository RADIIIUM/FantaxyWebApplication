using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class Post
    {
        public Post()
        {
            DisikesPosts = new HashSet<DisikesPost>();
            PostFiles = new HashSet<PostFile>();
        }

        public int IdPost { get; set; }
        public int? IdPlanet { get; set; }
        public string? OwnerLogin { get; set; }
        public bool IsPinned { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual User? OwnerLoginNavigation { get; set; }
        public virtual PostsInfo? PostsInfo { get; set; }
        public virtual ICollection<DisikesPost> DisikesPosts { get; set; }
        public virtual ICollection<PostFile> PostFiles { get; set; }
    }
}
