using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class ChatsInfo
    {
        public int IdChat { get; set; }
        public string ChatName { get; set; } = null!;
        public string? ChatDescription { get; set; }
        public string? Avatar { get; set; }
        public string? Background { get; set; }

        public virtual Chat IdChatNavigation { get; set; } = null!;
    }
}
