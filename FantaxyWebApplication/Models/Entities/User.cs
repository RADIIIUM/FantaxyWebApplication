using System;
using System.Collections.Generic;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class User
    {
        public User()
        {
            ChatMessages = new HashSet<ChatMessage>();
            Chats = new HashSet<Chat>();
            ChatsUsersChatRoles = new HashSet<ChatsUsersChatRole>();
            Comments = new HashSet<Comment>();
            GlobalRoleUsers = new HashSet<GlobalRoleUser>();
            LikeDislikeComments = new HashSet<LikeDislikeComment>();
            LikeDislikePosts = new HashSet<LikeDislikePost>();
            PlanetCuratorLoginNavigations = new HashSet<Planet>();
            PlanetOwnerLoginNavigations = new HashSet<Planet>();
            PlanetPlanetRoleUsers = new HashSet<PlanetPlanetRoleUser>();
            PlanetUsers = new HashSet<PlanetUser>();
            PlanetUsersInfos = new HashSet<PlanetUsersInfo>();
            Posts = new HashSet<Post>();
            UserStatusesStatuses = new HashSet<UserStatusesStatus>();
        }

        public string UserLogin { get; set; } = null!;
        public string UserPassword { get; set; } = null!;

        public virtual GlobalUsersInfo? GlobalUsersInfo { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<ChatsUsersChatRole> ChatsUsersChatRoles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GlobalRoleUser> GlobalRoleUsers { get; set; }
        public virtual ICollection<LikeDislikeComment> LikeDislikeComments { get; set; }
        public virtual ICollection<LikeDislikePost> LikeDislikePosts { get; set; }
        public virtual ICollection<Planet> PlanetCuratorLoginNavigations { get; set; }
        public virtual ICollection<Planet> PlanetOwnerLoginNavigations { get; set; }
        public virtual ICollection<PlanetPlanetRoleUser> PlanetPlanetRoleUsers { get; set; }
        public virtual ICollection<PlanetUser> PlanetUsers { get; set; }
        public virtual ICollection<PlanetUsersInfo> PlanetUsersInfos { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserStatusesStatus> UserStatusesStatuses { get; set; }
    }
}
