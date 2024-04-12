﻿using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class GlobalUsersInfo
    {
        public string UserLogin { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? UserEmail { get; set; }
        public string? UserTelephone { get; set; }
        public string? UserDescription { get; set; }
        public string? Avatar { get; set; }
        public string? MainBackground { get; set; }
        public string? ProfileBackground { get; set; }

        public virtual User UserLoginNavigation { get; set; } = null!;
    }
}
