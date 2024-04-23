using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class Planet
    {
        public Planet()
        {
            Chats = new HashSet<Chat>();
            DisikesPlanets = new HashSet<DisikesPlanet>();
            LikesPlanets = new HashSet<LikesPlanet>();
            PlanetPlanetRoleUsers = new HashSet<PlanetPlanetRoleUser>();
            PlanetUsers = new HashSet<PlanetUser>();
            PlanetUsersInfos = new HashSet<PlanetUsersInfo>();
            Posts = new HashSet<Post>();
            StatusesChats = new HashSet<StatusesChat>();
            StatusesPlanets = new HashSet<StatusesPlanet>();
        }

        public int IdPlanet { get; set; }
        public string? OwnerLogin { get; set; }
        public string? CuratorLogin { get; set; }

        public virtual User? CuratorLoginNavigation { get; set; }
        public virtual User? OwnerLoginNavigation { get; set; }
        public virtual PlanetInfo? PlanetInfo { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<DisikesPlanet> DisikesPlanets { get; set; }
        public virtual ICollection<LikesPlanet> LikesPlanets { get; set; }
        public virtual ICollection<PlanetPlanetRoleUser> PlanetPlanetRoleUsers { get; set; }
        public virtual ICollection<PlanetUser> PlanetUsers { get; set; }
        public virtual ICollection<PlanetUsersInfo> PlanetUsersInfos { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<StatusesChat> StatusesChats { get; set; }
        public virtual ICollection<StatusesPlanet> StatusesPlanets { get; set; }
    }
}
