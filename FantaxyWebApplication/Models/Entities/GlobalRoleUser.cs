using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class GlobalRoleUser
    {
        public int IdGlRU { get; set; }
        public string? UserLogin { get; set; }
        public int? IdRole { get; set; }

        public virtual GlobalRole? IdRoleNavigation { get; set; }
        public virtual User? UserLoginNavigation { get; set; }
    }
}
