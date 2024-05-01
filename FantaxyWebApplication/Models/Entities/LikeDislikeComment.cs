using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class LikeDislikeComment
    {
        public int IdLdc { get; set; }
        public bool LikeOrDislike { get; set; }
        public string UserLogin { get; set; } = null!;
        public int? IdComment { get; set; }

        public virtual Comment? IdCommentNavigation { get; set; }
        public virtual User UserLoginNavigation { get; set; } = null!;
    }
}
