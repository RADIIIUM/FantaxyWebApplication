using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class LikesPost
    {
        public string? LoginOwner { get; set; }
        public int? IdPost { get; set; }

        public virtual Post? IdPostNavigation { get; set; }
        public virtual User? LoginOwnerNavigation { get; set; }
    }
}
