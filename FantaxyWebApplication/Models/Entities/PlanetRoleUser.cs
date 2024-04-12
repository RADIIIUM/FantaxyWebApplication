using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetRoleUser
    {
        public int IdPlRU { get; set; }
        public string? UserLogin { get; set; }
        public int? IdRole { get; set; }

        public virtual PlanetRole? IdRoleNavigation { get; set; }
        public virtual User? UserLoginNavigation { get; set; }
    }
}
