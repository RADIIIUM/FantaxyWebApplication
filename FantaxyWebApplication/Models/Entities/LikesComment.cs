using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class LikesComment
    {
        public int IdLikes { get; set; }
        public string? LoginOwner { get; set; }
        public int? IdComment { get; set; }

        public virtual Comment? IdCommentNavigation { get; set; }
        public virtual User? LoginOwnerNavigation { get; set; }
    }
}
