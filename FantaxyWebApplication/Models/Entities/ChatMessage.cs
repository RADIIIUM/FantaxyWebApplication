using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class ChatMessage
    {
        public int IdChatMessages { get; set; }
        public string? LoginOwner { get; set; }
        public int? IdChat { get; set; }
        public string MessageText { get; set; } = null!;

        public virtual Chat? IdChatNavigation { get; set; }
        public virtual User? LoginOwnerNavigation { get; set; }
    }
}
