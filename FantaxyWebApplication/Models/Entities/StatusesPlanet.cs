using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class StatusesPlanet
    {
        public int IdStPl { get; set; }
        public int? IdPlanet { get; set; }
        public int? IdStatus { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual Status? IdStatusNavigation { get; set; }
    }
}
