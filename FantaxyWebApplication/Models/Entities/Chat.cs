using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class Chat
    {
        public Chat()
        {
            ChatFiles = new HashSet<ChatFile>();
        }

        public int IdChat { get; set; }
        public int? IdPlanet { get; set; }
        public string? OwnerLogin { get; set; }

        public virtual Planet? IdPlanetNavigation { get; set; }
        public virtual User? OwnerLoginNavigation { get; set; }
        public virtual ChatsInfo? ChatsInfo { get; set; }
        public virtual ICollection<ChatFile> ChatFiles { get; set; }
    }
}
