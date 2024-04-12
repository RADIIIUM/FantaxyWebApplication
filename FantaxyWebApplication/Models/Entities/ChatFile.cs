using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class ChatFile
    {
        public int IdChatFiles { get; set; }
        public int IdChat { get; set; }
        public string PathFile { get; set; } = null!;

        public virtual Chat IdChatNavigation { get; set; } = null!;
    }
}
