using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class LikeDislikePost
    {
        public int IdLdp { get; set; }
        public bool LikeOrDislike { get; set; }
        public string UserLogin { get; set; } = null!;
        public int? IdPost { get; set; }

        public virtual Post? IdPostNavigation { get; set; }
        public virtual User UserLoginNavigation { get; set; } = null!;
    }
}
