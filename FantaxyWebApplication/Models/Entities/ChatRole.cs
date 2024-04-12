using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class ChatRole
    {
        public ChatRole()
        {
            ChatsUsersChatRoles = new HashSet<ChatsUsersChatRole>();
        }

        public int IdChatRole { get; set; }
        public string ChatRole1 { get; set; } = null!;

        public virtual ICollection<ChatsUsersChatRole> ChatsUsersChatRoles { get; set; }
    }
}
