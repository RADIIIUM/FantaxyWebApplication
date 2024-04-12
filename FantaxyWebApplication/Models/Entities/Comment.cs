using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class Comment
    {
        public Comment()
        {
            CommentsFiles = new HashSet<CommentsFile>();
        }

        public int IdComment { get; set; }
        public string? OwnerLogin { get; set; }
        public string CommentText { get; set; } = null!;

        public virtual User? OwnerLoginNavigation { get; set; }
        public virtual ICollection<CommentsFile> CommentsFiles { get; set; }
    }
}
