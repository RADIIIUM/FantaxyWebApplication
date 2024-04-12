using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            UserStatusesStatuses = new HashSet<UserStatusesStatus>();
        }

        public int IdStatus { get; set; }
        public string NameStatus { get; set; } = null!;

        public virtual ICollection<UserStatusesStatus> UserStatusesStatuses { get; set; }
    }
}
