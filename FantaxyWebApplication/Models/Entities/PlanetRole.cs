using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetRole
    {
        public PlanetRole()
        {
            PlanetRoleUsers = new HashSet<PlanetRoleUser>();
        }

        public int IdRole { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<PlanetRoleUser> PlanetRoleUsers { get; set; }
    }
}
