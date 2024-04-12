using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class GlobalRole
    {
        public GlobalRole()
        {
            GlobalRoleUsers = new HashSet<GlobalRoleUser>();
        }

        public int IdRole { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<GlobalRoleUser> GlobalRoleUsers { get; set; }
    }
}
