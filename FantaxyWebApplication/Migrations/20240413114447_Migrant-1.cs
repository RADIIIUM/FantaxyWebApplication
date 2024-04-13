using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaxyWebApplication.Migrations
{
    public partial class Migrant1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRole",
                columns: table => new
                {
                    IdChatRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatRole__33BF2B4FB12D4334", x => x.IdChatRole);
                });

            migrationBuilder.CreateTable(
                name: "GlobalRole",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GlobalRo__B4369054D6C8C840", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "PlanetRole",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlanetRo__B4369054FD7EA7E3", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    IdStatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Statuses__B450643AB6A8176B", x => x.IdStatus);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__7F8E8D5F84BEA75E", x => x.UserLogin);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                columns: table => new
                {
                    IdStatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserStat__B450643A90CECD15", x => x.IdStatus);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    IdComment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommentText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comments__57C9AD58DEF939D9", x => x.IdComment);
                    table.ForeignKey(
                        name: "FK__Comments__OwnerL__7C4F7684",
                        column: x => x.OwnerLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GlobalRole_Users",
                columns: table => new
                {
                    IdGlR_U = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GlobalRo__187B74EF3CE7A76A", x => x.IdGlR_U);
                    table.ForeignKey(
                        name: "FK__GlobalRol__IdRol__4F7CD00D",
                        column: x => x.IdRole,
                        principalTable: "GlobalRole",
                        principalColumn: "IdRole");
                    table.ForeignKey(
                        name: "FK__GlobalRol__UserL__4E88ABD4",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GlobalUsersInfo",
                columns: table => new
                {
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserTelephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainBackground = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileBackground = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GlobalUs__7F8E8D5F5A0EC263", x => x.UserLogin);
                    table.ForeignKey(
                        name: "FK__GlobalUse__UserL__3C69FB99",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanetRole_Users",
                columns: table => new
                {
                    IdPlR_U = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlanetRo__475A5094A7C77AB6", x => x.IdPlR_U);
                    table.ForeignKey(
                        name: "FK__PlanetRol__IdRol__59063A47",
                        column: x => x.IdRole,
                        principalTable: "PlanetRole",
                        principalColumn: "IdRole");
                    table.ForeignKey(
                        name: "FK__PlanetRol__UserL__5812160E",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    IdPlanet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CuratorLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Planets__12FD4B354560ABC6", x => x.IdPlanet);
                    table.ForeignKey(
                        name: "FK__Planets__Curator__403A8C7D",
                        column: x => x.CuratorLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin");
                    table.ForeignKey(
                        name: "FK__Planets__OwnerLo__3F466844",
                        column: x => x.OwnerLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses_Statuses",
                columns: table => new
                {
                    IdUS_S = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdStatus = table.Column<int>(type: "int", nullable: true),
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserStat__BF8A4F8E2C09891B", x => x.IdUS_S);
                    table.ForeignKey(
                        name: "FK__UserStatu__IdSta__4AB81AF0",
                        column: x => x.IdStatus,
                        principalTable: "UserStatuses",
                        principalColumn: "IdStatus");
                    table.ForeignKey(
                        name: "FK__UserStatu__UserL__4BAC3F29",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentsFiles",
                columns: table => new
                {
                    IdCommentFiles = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdComment = table.Column<int>(type: "int", nullable: false),
                    PathFile = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comments__7D54E3F8B14EDFAC", x => x.IdCommentFiles);
                    table.ForeignKey(
                        name: "FK__CommentsF__IdCom__7F2BE32F",
                        column: x => x.IdComment,
                        principalTable: "Comments",
                        principalColumn: "IdComment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    IdChat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlanet = table.Column<int>(type: "int", nullable: true),
                    OwnerLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chats__3817F38CC76E5C7C", x => x.IdChat);
                    table.ForeignKey(
                        name: "FK__Chats__IdPlanet__5DCAEF64",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet");
                    table.ForeignKey(
                        name: "FK__Chats__OwnerLogi__5EBF139D",
                        column: x => x.OwnerLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes_Planets",
                columns: table => new
                {
                    IdLikes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginOwner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdPlanet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Likes_Pl__3FDC4886B5DE3F34", x => x.IdLikes);
                    table.ForeignKey(
                        name: "FK__Likes_Pla__IdPla__06CD04F7",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet");
                    table.ForeignKey(
                        name: "FK__Likes_Pla__Login__05D8E0BE",
                        column: x => x.LoginOwner,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planet_Users",
                columns: table => new
                {
                    IdPl_U = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdPlanet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Planet_U__FB8132F5326A75DA", x => x.IdPl_U);
                    table.ForeignKey(
                        name: "FK__Planet_Us__IdPla__5535A963",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet");
                    table.ForeignKey(
                        name: "FK__Planet_Us__UserL__5441852A",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanetInfo",
                columns: table => new
                {
                    IdPlanet = table.Column<int>(type: "int", nullable: false),
                    PlanetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlanetDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProfileBackground = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MainBackground = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__PlanetInf__IdPla__0E6E26BF",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanetUsersInfo",
                columns: table => new
                {
                    idPlUsInfo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdPlanet = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UserDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MainBackground = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProfileBackground = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlanetUs__75271A21F6503D47", x => x.idPlUsInfo);
                    table.ForeignKey(
                        name: "FK__PlanetUse__IdPla__440B1D61",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet");
                    table.ForeignKey(
                        name: "FK__PlanetUse__UserL__4316F928",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    IdPost = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlanet = table.Column<int>(type: "int", nullable: true),
                    OwnerLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Posts__F8DCBD4D672470BC", x => x.IdPost);
                    table.ForeignKey(
                        name: "FK__Posts__IdPlanet__72C60C4A",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet");
                    table.ForeignKey(
                        name: "FK__Posts__OwnerLogi__73BA3083",
                        column: x => x.OwnerLogin,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statuses_Chats",
                columns: table => new
                {
                    IdSt_Pl = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlanet = table.Column<int>(type: "int", nullable: true),
                    IdStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Statuses__2E9E5A44374573E4", x => x.IdSt_Pl);
                    table.ForeignKey(
                        name: "FK__Statuses___IdPla__6EF57B66",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Statuses___IdSta__6FE99F9F",
                        column: x => x.IdStatus,
                        principalTable: "Statuses",
                        principalColumn: "IdStatus");
                });

            migrationBuilder.CreateTable(
                name: "Statuses_Planet",
                columns: table => new
                {
                    IdSt_Pl = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlanet = table.Column<int>(type: "int", nullable: true),
                    IdStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Statuses__2E9E5A440026F635", x => x.IdSt_Pl);
                    table.ForeignKey(
                        name: "FK__Statuses___IdPla__6B24EA82",
                        column: x => x.IdPlanet,
                        principalTable: "Planets",
                        principalColumn: "IdPlanet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Statuses___IdSta__6C190EBB",
                        column: x => x.IdStatus,
                        principalTable: "Statuses",
                        principalColumn: "IdStatus");
                });

            migrationBuilder.CreateTable(
                name: "ChatFiles",
                columns: table => new
                {
                    IdChatFiles = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdChat = table.Column<int>(type: "int", nullable: false),
                    PathFile = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatFile__9AE1324E131733DB", x => x.IdChatFiles);
                    table.ForeignKey(
                        name: "FK__ChatFiles__IdCha__797309D9",
                        column: x => x.IdChat,
                        principalTable: "Chats",
                        principalColumn: "IdChat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    IdChatMessages = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginOwner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdChat = table.Column<int>(type: "int", nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatMess__7909324CC7C01757", x => x.IdChatMessages);
                    table.ForeignKey(
                        name: "FK__ChatMessa__IdCha__02FC7413",
                        column: x => x.IdChat,
                        principalTable: "Chats",
                        principalColumn: "IdChat");
                    table.ForeignKey(
                        name: "FK__ChatMessa__Login__02084FDA",
                        column: x => x.LoginOwner,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats_Users_ChatRole",
                columns: table => new
                {
                    IdC_U = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdChat = table.Column<int>(type: "int", nullable: true),
                    LoginUsers = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdChatRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chats_Us__0FA070803DFA5C56", x => x.IdC_U);
                    table.ForeignKey(
                        name: "FK__Chats_Use__IdCha__6383C8BA",
                        column: x => x.IdChat,
                        principalTable: "Chats",
                        principalColumn: "IdChat");
                    table.ForeignKey(
                        name: "FK__Chats_Use__IdCha__656C112C",
                        column: x => x.IdChatRole,
                        principalTable: "ChatRole",
                        principalColumn: "IdChatRole");
                    table.ForeignKey(
                        name: "FK__Chats_Use__Login__6477ECF3",
                        column: x => x.LoginUsers,
                        principalTable: "Users",
                        principalColumn: "UserLogin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatsInfo",
                columns: table => new
                {
                    IdChat = table.Column<int>(type: "int", nullable: false),
                    ChatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChatDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Background = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatsInf__3817F38C531E447C", x => x.IdChat);
                    table.ForeignKey(
                        name: "FK__ChatsInfo__IdCha__68487DD7",
                        column: x => x.IdChat,
                        principalTable: "Chats",
                        principalColumn: "IdChat");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatFiles_IdChat",
                table: "ChatFiles",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_IdChat",
                table: "ChatMessages",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_LoginOwner",
                table: "ChatMessages",
                column: "LoginOwner");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_IdPlanet",
                table: "Chats",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_OwnerLogin",
                table: "Chats",
                column: "OwnerLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Users_ChatRole_IdChat",
                table: "Chats_Users_ChatRole",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Users_ChatRole_IdChatRole",
                table: "Chats_Users_ChatRole",
                column: "IdChatRole");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Users_ChatRole_LoginUsers",
                table: "Chats_Users_ChatRole",
                column: "LoginUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OwnerLogin",
                table: "Comments",
                column: "OwnerLogin");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsFiles_IdComment",
                table: "CommentsFiles",
                column: "IdComment");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalRole_Users_IdRole",
                table: "GlobalRole_Users",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalRole_Users_UserLogin",
                table: "GlobalRole_Users",
                column: "UserLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Planets_IdPlanet",
                table: "Likes_Planets",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Planets_LoginOwner",
                table: "Likes_Planets",
                column: "LoginOwner");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_Users_IdPlanet",
                table: "Planet_Users",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_Users_UserLogin",
                table: "Planet_Users",
                column: "UserLogin");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetInfo_IdPlanet",
                table: "PlanetInfo",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetRole_Users_IdRole",
                table: "PlanetRole_Users",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetRole_Users_UserLogin",
                table: "PlanetRole_Users",
                column: "UserLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_CuratorLogin",
                table: "Planets",
                column: "CuratorLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_OwnerLogin",
                table: "Planets",
                column: "OwnerLogin");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetUsersInfo_IdPlanet",
                table: "PlanetUsersInfo",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetUsersInfo_UserLogin",
                table: "PlanetUsersInfo",
                column: "UserLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IdPlanet",
                table: "Posts",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerLogin",
                table: "Posts",
                column: "OwnerLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Chats_IdPlanet",
                table: "Statuses_Chats",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Chats_IdStatus",
                table: "Statuses_Chats",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Planet_IdPlanet",
                table: "Statuses_Planet",
                column: "IdPlanet");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Planet_IdStatus",
                table: "Statuses_Planet",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_Statuses_IdStatus",
                table: "UserStatuses_Statuses",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_Statuses_UserLogin",
                table: "UserStatuses_Statuses",
                column: "UserLogin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatFiles");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Chats_Users_ChatRole");

            migrationBuilder.DropTable(
                name: "ChatsInfo");

            migrationBuilder.DropTable(
                name: "CommentsFiles");

            migrationBuilder.DropTable(
                name: "GlobalRole_Users");

            migrationBuilder.DropTable(
                name: "GlobalUsersInfo");

            migrationBuilder.DropTable(
                name: "Likes_Planets");

            migrationBuilder.DropTable(
                name: "Planet_Users");

            migrationBuilder.DropTable(
                name: "PlanetInfo");

            migrationBuilder.DropTable(
                name: "PlanetRole_Users");

            migrationBuilder.DropTable(
                name: "PlanetUsersInfo");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Statuses_Chats");

            migrationBuilder.DropTable(
                name: "Statuses_Planet");

            migrationBuilder.DropTable(
                name: "UserStatuses_Statuses");

            migrationBuilder.DropTable(
                name: "ChatRole");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GlobalRole");

            migrationBuilder.DropTable(
                name: "PlanetRole");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "UserStatuses");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
