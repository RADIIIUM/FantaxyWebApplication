using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class ChatsUsersChatRole
    {
        public int IdCU { get; set; }
        public int? IdChat { get; set; }
        public string? LoginUsers { get; set; }
        public int? IdChatRole { get; set; }

        public virtual Chat? IdChatNavigation { get; set; }
        public virtual ChatRole? IdChatRoleNavigation { get; set; }
        public virtual User? LoginUsersNavigation { get; set; }
    }
}
