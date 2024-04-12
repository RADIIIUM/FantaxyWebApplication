using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class Post
    {
        public int IdPost { get; set; }
        public int? IdPlanet { get; set; }
        public string? OwnerLogin { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual User? OwnerLoginNavigation { get; set; }
    }
}
