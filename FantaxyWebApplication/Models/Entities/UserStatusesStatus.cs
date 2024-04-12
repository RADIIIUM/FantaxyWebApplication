using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class UserStatusesStatus
    {
        public int IdUsS { get; set; }
        public int? IdStatus { get; set; }
        public string? UserLogin { get; set; }

        public virtual UserStatus? IdStatusNavigation { get; set; }
        public virtual User? UserLoginNavigation { get; set; }
    }
}
