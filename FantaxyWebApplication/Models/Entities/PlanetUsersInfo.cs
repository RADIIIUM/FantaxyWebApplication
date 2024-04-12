using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetUsersInfo
    {
        public int IdPlUsInfo { get; set; }
        public string UserLogin { get; set; } = null!;
        public int? IdPlanet { get; set; }
        public string UserName { get; set; } = null!;
        public string UserDescription { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? MainBackground { get; set; }
        public string? ProfileBackground { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual User UserLoginNavigation { get; set; } = null!;
    }
}
