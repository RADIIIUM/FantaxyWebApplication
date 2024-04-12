using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class Status
    {
        public Status()
        {
            StatusesChats = new HashSet<StatusesChat>();
            StatusesPlanets = new HashSet<StatusesPlanet>();
        }

        public int IdStatus { get; set; }
        public string? StatusName { get; set; }

        public virtual ICollection<StatusesChat> StatusesChats { get; set; }
        public virtual ICollection<StatusesPlanet> StatusesPlanets { get; set; }
    }
}
