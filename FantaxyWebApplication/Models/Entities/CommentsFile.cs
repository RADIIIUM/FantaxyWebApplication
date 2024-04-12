using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class CommentsFile
    {
        public int IdCommentFiles { get; set; }
        public int IdComment { get; set; }
        public string PathFile { get; set; } = null!;

        public virtual Comment IdCommentNavigation { get; set; } = null!;
    }
}
