using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetInfo
    {
        public int IdPlanet { get; set; }
        public string PlanetName { get; set; } = null!;
        public string? PlanetDescription { get; set; }
        public string Avatar { get; set; } = null!;
        public string ProfileBackground { get; set; } = null!;
        public string MainBackground { get; set; } = null!;

        public virtual Planet IdPlanetNavigation { get; set; } = null!;
    }
}
