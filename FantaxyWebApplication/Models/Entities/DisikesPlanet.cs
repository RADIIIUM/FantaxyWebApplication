using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class DisikesPlanet
    {
        public int IdDislikes { get; set; }
        public string? LoginOwner { get; set; }
        public int? IdPlanet { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual User? LoginOwnerNavigation { get; set; }
    }
}
