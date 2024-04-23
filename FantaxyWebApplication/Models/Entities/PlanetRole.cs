using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetRole
    {
        public PlanetRole()
        {
            PlanetPlanetRoleUsers = new HashSet<PlanetPlanetRoleUser>();
        }

        public int IdRole { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<PlanetPlanetRoleUser> PlanetPlanetRoleUsers { get; set; }
    }
}
