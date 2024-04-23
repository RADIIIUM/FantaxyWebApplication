using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PostFile
    {
        public int IdPostFile { get; set; }
        public int IdPost { get; set; }
        public string PathFile { get; set; } = null!;

        public virtual Post IdPostNavigation { get; set; } = null!;
    }
}
