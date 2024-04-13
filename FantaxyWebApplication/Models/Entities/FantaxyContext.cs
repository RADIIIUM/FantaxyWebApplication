using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class FantaxyContext : DbContext
    {
        public FantaxyContext()
        {
        }

        public FantaxyContext(DbContextOptions<FantaxyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatFile> ChatFiles { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<ChatRole> ChatRoles { get; set; } = null!;
        public virtual DbSet<ChatsInfo> ChatsInfos { get; set; } = null!;
        public virtual DbSet<ChatsUsersChatRole> ChatsUsersChatRoles { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CommentsFile> CommentsFiles { get; set; } = null!;
        public virtual DbSet<GlobalRole> GlobalRoles { get; set; } = null!;
        public virtual DbSet<GlobalRoleUser> GlobalRoleUsers { get; set; } = null!;
        public virtual DbSet<GlobalUsersInfo> GlobalUsersInfos { get; set; } = null!;
        public virtual DbSet<LikesPlanet> LikesPlanets { get; set; } = null!;
        public virtual DbSet<Planet> Planets { get; set; } = null!;
        public virtual DbSet<PlanetInfo> PlanetInfos { get; set; } = null!;
        public virtual DbSet<PlanetRole> PlanetRoles { get; set; } = null!;
        public virtual DbSet<PlanetRoleUser> PlanetRoleUsers { get; set; } = null!;
        public virtual DbSet<PlanetUser> PlanetUsers { get; set; } = null!;
        public virtual DbSet<PlanetUsersInfo> PlanetUsersInfos { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<StatusesChat> StatusesChats { get; set; } = null!;
        public virtual DbSet<StatusesPlanet> StatusesPlanets { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserStatus> UserStatuses { get; set; } = null!;
        public virtual DbSet<UserStatusesStatus> UserStatusesStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-NNBEJC9;Database=Fantaxy;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.IdChat)
                    .HasName("PK__Chats__3817F38CC76E5C7C");

                entity.Property(e => e.OwnerLogin).HasMaxLength(50);

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.IdPlanet)
                    .HasConstraintName("FK__Chats__IdPlanet__5DCAEF64");

                entity.HasOne(d => d.OwnerLoginNavigation)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.OwnerLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Chats__OwnerLogi__5EBF139D");
            });

            modelBuilder.Entity<ChatFile>(entity =>
            {
                entity.HasKey(e => e.IdChatFiles)
                    .HasName("PK__ChatFile__9AE1324E131733DB");

                entity.Property(e => e.PathFile).HasMaxLength(1000);

                entity.HasOne(d => d.IdChatNavigation)
                    .WithMany(p => p.ChatFiles)
                    .HasForeignKey(d => d.IdChat)
                    .HasConstraintName("FK__ChatFiles__IdCha__797309D9");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.IdChatMessages)
                    .HasName("PK__ChatMess__7909324CC7C01757");

                entity.Property(e => e.LoginOwner).HasMaxLength(50);

                entity.Property(e => e.MessageText).HasMaxLength(2000);

                entity.HasOne(d => d.IdChatNavigation)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.IdChat)
                    .HasConstraintName("FK__ChatMessa__IdCha__02FC7413");

                entity.HasOne(d => d.LoginOwnerNavigation)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.LoginOwner)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ChatMessa__Login__02084FDA");
            });

            modelBuilder.Entity<ChatRole>(entity =>
            {
                entity.HasKey(e => e.IdChatRole)
                    .HasName("PK__ChatRole__33BF2B4FB12D4334");

                entity.ToTable("ChatRole");

                entity.Property(e => e.ChatRole1)
                    .HasMaxLength(50)
                    .HasColumnName("ChatRole");
            });

            modelBuilder.Entity<ChatsInfo>(entity =>
            {
                entity.HasKey(e => e.IdChat)
                    .HasName("PK__ChatsInf__3817F38C531E447C");

                entity.ToTable("ChatsInfo");

                entity.Property(e => e.IdChat).ValueGeneratedOnAdd();

                entity.Property(e => e.Avatar).HasMaxLength(1000);

                entity.Property(e => e.Background).HasMaxLength(1000);

                entity.Property(e => e.ChatDescription).HasMaxLength(2000);

                entity.Property(e => e.ChatName).HasMaxLength(50);

                entity.HasOne(d => d.IdChatNavigation)
                    .WithOne(p => p.ChatsInfo)
                    .HasForeignKey<ChatsInfo>(d => d.IdChat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChatsInfo__IdCha__68487DD7");
            });

            modelBuilder.Entity<ChatsUsersChatRole>(entity =>
            {
                entity.HasKey(e => e.IdCU)
                    .HasName("PK__Chats_Us__0FA070803DFA5C56");

                entity.ToTable("Chats_Users_ChatRole");

                entity.Property(e => e.IdCU).HasColumnName("IdC_U");

                entity.Property(e => e.LoginUsers).HasMaxLength(50);

                entity.HasOne(d => d.IdChatNavigation)
                    .WithMany(p => p.ChatsUsersChatRoles)
                    .HasForeignKey(d => d.IdChat)
                    .HasConstraintName("FK__Chats_Use__IdCha__6383C8BA");

                entity.HasOne(d => d.IdChatRoleNavigation)
                    .WithMany(p => p.ChatsUsersChatRoles)
                    .HasForeignKey(d => d.IdChatRole)
                    .HasConstraintName("FK__Chats_Use__IdCha__656C112C");

                entity.HasOne(d => d.LoginUsersNavigation)
                    .WithMany(p => p.ChatsUsersChatRoles)
                    .HasForeignKey(d => d.LoginUsers)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Chats_Use__Login__6477ECF3");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComment)
                    .HasName("PK__Comments__57C9AD58DEF939D9");

                entity.Property(e => e.CommentText).HasMaxLength(2000);

                entity.Property(e => e.OwnerLogin).HasMaxLength(50);

                entity.HasOne(d => d.OwnerLoginNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.OwnerLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Comments__OwnerL__7C4F7684");
            });

            modelBuilder.Entity<CommentsFile>(entity =>
            {
                entity.HasKey(e => e.IdCommentFiles)
                    .HasName("PK__Comments__7D54E3F8B14EDFAC");

                entity.Property(e => e.PathFile).HasMaxLength(1000);

                entity.HasOne(d => d.IdCommentNavigation)
                    .WithMany(p => p.CommentsFiles)
                    .HasForeignKey(d => d.IdComment)
                    .HasConstraintName("FK__CommentsF__IdCom__7F2BE32F");
            });

            modelBuilder.Entity<GlobalRole>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__GlobalRo__B4369054D6C8C840");

                entity.ToTable("GlobalRole");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<GlobalRoleUser>(entity =>
            {
                entity.HasKey(e => e.IdGlRU)
                    .HasName("PK__GlobalRo__187B74EF3CE7A76A");

                entity.ToTable("GlobalRole_Users");

                entity.Property(e => e.IdGlRU).HasColumnName("IdGlR_U");

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.GlobalRoleUsers)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK__GlobalRol__IdRol__4F7CD00D");

                entity.HasOne(d => d.UserLoginNavigation)
                    .WithMany(p => p.GlobalRoleUsers)
                    .HasForeignKey(d => d.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__GlobalRol__UserL__4E88ABD4");
            });

            modelBuilder.Entity<GlobalUsersInfo>(entity =>
            {
                entity.HasKey(e => e.UserLogin)
                    .HasName("PK__GlobalUs__7F8E8D5F5A0EC263");

                entity.ToTable("GlobalUsersInfo");

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.Property(e => e.UserDescription).HasMaxLength(2000);

                entity.Property(e => e.UserEmail).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(40);

                entity.Property(e => e.UserTelephone).HasMaxLength(20);

                entity.HasOne(d => d.UserLoginNavigation)
                    .WithOne(p => p.GlobalUsersInfo)
                    .HasForeignKey<GlobalUsersInfo>(d => d.UserLogin)
                    .HasConstraintName("FK__GlobalUse__UserL__3C69FB99");
            });

            modelBuilder.Entity<LikesPlanet>(entity =>
            {
                entity.HasKey(e => e.IdLikes)
                    .HasName("PK__Likes_Pl__3FDC4886B5DE3F34");

                entity.ToTable("Likes_Planets");

                entity.Property(e => e.LoginOwner).HasMaxLength(50);

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.LikesPlanets)
                    .HasForeignKey(d => d.IdPlanet)
                    .HasConstraintName("FK__Likes_Pla__IdPla__06CD04F7");

                entity.HasOne(d => d.LoginOwnerNavigation)
                    .WithMany(p => p.LikesPlanets)
                    .HasForeignKey(d => d.LoginOwner)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Likes_Pla__Login__05D8E0BE");
            });

            modelBuilder.Entity<Planet>(entity =>
            {
                entity.HasKey(e => e.IdPlanet)
                    .HasName("PK__Planets__12FD4B354560ABC6");

                entity.Property(e => e.CuratorLogin).HasMaxLength(50);

                entity.Property(e => e.OwnerLogin).HasMaxLength(50);

                entity.HasOne(d => d.CuratorLoginNavigation)
                    .WithMany(p => p.PlanetCuratorLoginNavigations)
                    .HasForeignKey(d => d.CuratorLogin)
                    .HasConstraintName("FK__Planets__Curator__403A8C7D");

                entity.HasOne(d => d.OwnerLoginNavigation)
                    .WithMany(p => p.PlanetOwnerLoginNavigations)
                    .HasForeignKey(d => d.OwnerLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Planets__OwnerLo__3F466844");
            });

            modelBuilder.Entity<PlanetInfo>(entity =>
            {
                entity.HasKey(e => e.IdPlanet)
                    .HasName("PK__PlanetIn__12FD4B35341428FF");

                entity.ToTable("PlanetInfo");

                entity.Property(e => e.IdPlanet).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(200);

                entity.Property(e => e.MainBackground).HasMaxLength(200);

                entity.Property(e => e.PlanetDescription).HasMaxLength(4000);

                entity.Property(e => e.PlanetName).HasMaxLength(50);

                entity.Property(e => e.ProfileBackground).HasMaxLength(200);

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithOne(p => p.PlanetInfo)
                    .HasForeignKey<PlanetInfo>(d => d.IdPlanet)
                    .HasConstraintName("FK__PlanetInf__IdPla__14270015");
            });

            modelBuilder.Entity<PlanetRole>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__PlanetRo__B4369054FD7EA7E3");

                entity.ToTable("PlanetRole");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<PlanetRoleUser>(entity =>
            {
                entity.HasKey(e => e.IdPlRU)
                    .HasName("PK__PlanetRo__475A5094A7C77AB6");

                entity.ToTable("PlanetRole_Users");

                entity.Property(e => e.IdPlRU).HasColumnName("IdPlR_U");

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.PlanetRoleUsers)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK__PlanetRol__IdRol__59063A47");

                entity.HasOne(d => d.UserLoginNavigation)
                    .WithMany(p => p.PlanetRoleUsers)
                    .HasForeignKey(d => d.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PlanetRol__UserL__5812160E");
            });

            modelBuilder.Entity<PlanetUser>(entity =>
            {
                entity.HasKey(e => e.IdPlU)
                    .HasName("PK__Planet_U__FB8132F5326A75DA");

                entity.ToTable("Planet_Users");

                entity.Property(e => e.IdPlU).HasColumnName("IdPl_U");

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.PlanetUsers)
                    .HasForeignKey(d => d.IdPlanet)
                    .HasConstraintName("FK__Planet_Us__IdPla__5535A963");

                entity.HasOne(d => d.UserLoginNavigation)
                    .WithMany(p => p.PlanetUsers)
                    .HasForeignKey(d => d.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Planet_Us__UserL__5441852A");
            });

            modelBuilder.Entity<PlanetUsersInfo>(entity =>
            {
                entity.HasKey(e => e.IdPlUsInfo)
                    .HasName("PK__PlanetUs__75271A21F6503D47");

                entity.ToTable("PlanetUsersInfo");

                entity.Property(e => e.IdPlUsInfo).HasColumnName("idPlUsInfo");

                entity.Property(e => e.Avatar).HasMaxLength(1000);

                entity.Property(e => e.MainBackground).HasMaxLength(1000);

                entity.Property(e => e.ProfileBackground).HasMaxLength(1000);

                entity.Property(e => e.UserDescription).HasMaxLength(50);

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(40);

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.PlanetUsersInfos)
                    .HasForeignKey(d => d.IdPlanet)
                    .HasConstraintName("FK__PlanetUse__IdPla__440B1D61");

                entity.HasOne(d => d.UserLoginNavigation)
                    .WithMany(p => p.PlanetUsersInfos)
                    .HasForeignKey(d => d.UserLogin)
                    .HasConstraintName("FK__PlanetUse__UserL__4316F928");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost)
                    .HasName("PK__Posts__F8DCBD4D672470BC");

                entity.Property(e => e.OwnerLogin).HasMaxLength(50);

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.IdPlanet)
                    .HasConstraintName("FK__Posts__IdPlanet__72C60C4A");

                entity.HasOne(d => d.OwnerLoginNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.OwnerLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Posts__OwnerLogi__73BA3083");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.IdStatus)
                    .HasName("PK__Statuses__B450643AB6A8176B");

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<StatusesChat>(entity =>
            {
                entity.HasKey(e => e.IdStPl)
                    .HasName("PK__Statuses__2E9E5A44374573E4");

                entity.ToTable("Statuses_Chats");

                entity.Property(e => e.IdStPl).HasColumnName("IdSt_Pl");

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.StatusesChats)
                    .HasForeignKey(d => d.IdPlanet)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Statuses___IdPla__6EF57B66");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.StatusesChats)
                    .HasForeignKey(d => d.IdStatus)
                    .HasConstraintName("FK__Statuses___IdSta__6FE99F9F");
            });

            modelBuilder.Entity<StatusesPlanet>(entity =>
            {
                entity.HasKey(e => e.IdStPl)
                    .HasName("PK__Statuses__2E9E5A440026F635");

                entity.ToTable("Statuses_Planet");

                entity.Property(e => e.IdStPl).HasColumnName("IdSt_Pl");

                entity.HasOne(d => d.IdPlanetNavigation)
                    .WithMany(p => p.StatusesPlanets)
                    .HasForeignKey(d => d.IdPlanet)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Statuses___IdPla__6B24EA82");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.StatusesPlanets)
                    .HasForeignKey(d => d.IdStatus)
                    .HasConstraintName("FK__Statuses___IdSta__6C190EBB");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserLogin)
                    .HasName("PK__Users__7F8E8D5F84BEA75E");

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.Property(e => e.UserPassword).HasMaxLength(40);
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.HasKey(e => e.IdStatus)
                    .HasName("PK__UserStat__B450643A90CECD15");

                entity.Property(e => e.NameStatus).HasMaxLength(50);
            });

            modelBuilder.Entity<UserStatusesStatus>(entity =>
            {
                entity.HasKey(e => e.IdUsS)
                    .HasName("PK__UserStat__BF8A4F8E2C09891B");

                entity.ToTable("UserStatuses_Statuses");

                entity.Property(e => e.IdUsS).HasColumnName("IdUS_S");

                entity.Property(e => e.UserLogin).HasMaxLength(50);

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.UserStatusesStatuses)
                    .HasForeignKey(d => d.IdStatus)
                    .HasConstraintName("FK__UserStatu__IdSta__4AB81AF0");

                entity.HasOne(d => d.UserLoginNavigation)
                    .WithMany(p => p.UserStatusesStatuses)
                    .HasForeignKey(d => d.UserLogin)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UserStatu__UserL__4BAC3F29");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
