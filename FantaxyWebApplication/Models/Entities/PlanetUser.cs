using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetUser
    {
        public int IdPlU { get; set; }
        public string? UserLogin { get; set; }
        public int? IdPlanet { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual User? UserLoginNavigation { get; set; }
    }
}
