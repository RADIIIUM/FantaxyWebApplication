using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Models;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Web.Helpers;
using Microsoft.Data.SqlClient;

namespace FantaxyWebApplication.Controllers
{
    public class ChatController : Controller
    {

        FantaxyContext _db;
        IWebHostEnvironment _webHostEnvironment;
        public ChatController(FantaxyContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Chat(int IdChat)
        {
            int PlanetId = HttpContext.Session.Get<int>("PlanetId");
            var json3 = HttpContext.Request.Cookies[$"Profile_{PlanetId}"];
            UserModel? profilePlanet = JsonSerializer.Deserialize<UserModel>(json3);
            var query = from p in _db.Chats
                             where p.IdChat == IdChat
                             join u in _db.ChatsInfos on p.IdChat equals u.IdChat into up
                             from u in up.DefaultIfEmpty()
                             select new ChatModel()
                             {
                                 IdChat = p.IdChat,
                                 Name = u.ChatName,
                                 Desc = u.ChatDescription,
                                 MainBackground = u.Background,
                                 ProfileBackground = u.Avatar,
                                 MembersCount = _db.ChatsUsersChatRoles.Count(x => x.IdChat == p.IdChat),
                                 OwnerLogin = p.OwnerLogin,
                                 IdPlanet = PlanetId,
                             };
            ChatModel? chat = query.FirstOrDefault();
            if(chat != null) 
            {

                ChatsUsersChatRole? profile = _db.ChatsUsersChatRoles.FirstOrDefault(x => x.IdChat == IdChat && profilePlanet.Login == x.LoginUsers);
                if(profile == null)
                {
                    profile = await CreateChatProfile(profilePlanet.Login, chat.IdChat);
                }
                if(profile.IdChatRole == 5) return View("Banned");
                HttpContext.Session.Set<ChatsUsersChatRole>("ChatUser", profile);
                HttpContext.Session.Set<ChatModel>("Chat", chat);
                return View(chat);
            }
            else
            {
                return View("ChatList");
            }

        }

        public async Task<ChatsUsersChatRole> CreateChatProfile(string Login, int IdChat)
        {
            ChatsUsersChatRole chatProfile = new ChatsUsersChatRole();
            chatProfile.IdChat = IdChat;
            chatProfile.IdChatRole = 4;
            chatProfile.LoginUsers = Login;

            _db.ChatsUsersChatRoles.Add(chatProfile);
            await _db.SaveChangesAsync();
            return chatProfile;
        }

        [HttpPost]
        public async Task<JsonResult> BanChatUser([FromBody] UserModel model)
        {
                ChatsUsersChatRole? user = _db.ChatsUsersChatRoles.FirstOrDefault(x => x.LoginUsers == model.Login);
                if(user != null)
                {
                    if(user.IdChatRole == 1)
                    {
                        return Json(new { success = false });
                    }
                    user.IdChatRole = 5;
                    _db.ChatsUsersChatRoles.Update(user);
                    await _db.SaveChangesAsync();

                    return Json(new { success = true });
                }
                return Json(new { success = false });
        }

        public async Task<IActionResult> UserList()
        {
            return PartialView(await GetUserAsync());
        }

        public async Task<IActionResult> AdminUserList()
        {
            return PartialView(await GetUserAsync());
        }

        private async Task<IList<SearchModel>> GetUserAsync()
        {
            ChatModel chat = HttpContext.Session.Get<ChatModel>("Chat");
            var userOfChat = _db.ChatsUsersChatRoles.Where(x => x.IdChat == chat.IdChat).Join(_db.PlanetUsersInfos, y => y.LoginUsers,
                x => x.UserLogin, (x, y) => new SearchModel
                {
                    Id = x.LoginUsers,
                    Name = y.UserName,
                    RoleOrStatus = 4,
                    Avatar = y.Avatar,
                    Profile = y.ProfileBackground
                })
                .Distinct();
            userOfChat = userOfChat.Where(s => s.RoleOrStatus != 5);

            IList<SearchModel> list = await userOfChat.AsNoTracking().ToListAsync();
            return list;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChat (int IdChat)
        {
            int PlanetId = HttpContext.Session.Get<int>("PlanetId");
            Chat? chat = _db.Chats.FirstOrDefault(x => x.IdChat == IdChat && x.IdPlanet == PlanetId);
            if (chat != null)
            {
                using (var dbContextTransaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Database.ExecuteSqlRaw("DELETE FROM Chats WHERE IdChat = @IdChat", new SqlParameter("@IdChat", chat.IdChat));
                        dbContextTransaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        dbContextTransaction.Rollback();
                        // Обработка ошибки
                    }
                }
            }
            return RedirectToAction("ChatList", "Chat");
        }

        [HttpPost]
        public async Task<IActionResult> BanChat(int IdChat)
        {
            int PlanetId = HttpContext.Session.Get<int>("PlanetId");
            Chat? chat = _db.Chats.FirstOrDefault(x => x.IdChat == IdChat && x.IdPlanet == PlanetId);
            if (chat != null)
            {
                using (var dbContextTransaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Database.ExecuteSqlRaw("DELETE FROM Chats WHERE IdChat = @IdChat", new SqlParameter("@IdChat", chat.IdChat));
                        dbContextTransaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        dbContextTransaction.Rollback();
                        // Обработка ошибки
                    }
                }
            }
            return RedirectToAction("ChatList", "Chat");
        }

        public async Task<IActionResult> DetailChat()
        {
            ChatModel chat = HttpContext.Session.Get<ChatModel>("Chat");
            if(chat != null)
            {
                return View(chat);
            }
            return NotFound();
        }

        public async Task<IActionResult> EditChat()
        {
            var result = HttpContext.Session.Get<ChatCreate>("EditChat");
            if (result != null)
            {
                return View(result);
            }
            ChatModel edit = HttpContext.Session.Get<ChatModel>("Chat");
            ChatCreate edChat = new ChatCreate();
            edChat.Name = edit.Name;
            edChat.Desc = edit.Desc;
            edChat.ProfileBack = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_webHostEnvironment, edit.ProfileBackground));
            edChat.MainBackground = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_webHostEnvironment, edit.MainBackground));
            
            HttpContext.Session.Set<ChatCreate>("EditChat", edChat);
            return View(edChat);
        }

        [HttpGet]
        public async Task<IActionResult> MessagePartial()
        {
            int PlanetId = HttpContext.Session.Get<int>("PlanetId");
            var json = HttpContext.Request.Cookies[$"Profile_{PlanetId}"];

            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            ChatModel? model = HttpContext.Session.Get<ChatModel>("Chat");
            if(model != null)
            {
                IQueryable<MessageModel> query = 
                            from p in _db.ChatMessages where p.IdChat == model.IdChat
                            select new MessageModel()
                            {
                                owner = _db.PlanetUsersInfos.FirstOrDefault(x => x.UserLogin == p.LoginOwner).UserName,
                                avatar = _db.PlanetUsersInfos.FirstOrDefault(x => x.UserLogin == p.LoginOwner).Avatar,
                                message = p.MessageText,
                                IsAuthor = (p.LoginOwner == userModel.Login ?  true : false),
                            };

                IList<MessageModel> list = await query.AsNoTracking().ToListAsync();
                return PartialView(list);
            }
            return Redirect("/Chat/ChatList");
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage([FromBody] MessageModel message)
        {
            if(!string.IsNullOrEmpty(message.message))
            {
                ChatModel? chat = HttpContext.Session.Get<ChatModel>("Chat");
                int PlanetId = HttpContext.Session.Get<int>("PlanetId");
                var json = HttpContext.Request.Cookies[$"Profile_{PlanetId}"];
                UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

                if (chat != null && userModel != null)
                {
                    HttpContext.Session.Get<string>("Profile");
                    ChatMessage cm = new ChatMessage();
                    cm.IdChat = chat.IdChat;
                    cm.MessageText = message.message;
                    cm.LoginOwner = userModel.Login;
                    _db.ChatMessages.Add(cm);
                    await _db.SaveChangesAsync();
                    MessageModel responseMessage = new MessageModel
                    {
                        avatar = userModel.Avatar,
                        owner = userModel.Name,
                        message = message.message
                    };

                    return Json(responseMessage);
                }
            }
            return Json(new { success = false });
        }

        public IActionResult ChatList()
        {
            HttpContext.Session.Remove("CreateChat");
            return View();
        }

        public async Task<IActionResult> ChatPartial(int PlanetId)
        {
            PlanetId = HttpContext.Session.Get<int>("PlanetId");
            var chats = from p in _db.Chats
                        where p.IdPlanet == PlanetId
                        join u in _db.ChatsInfos on p.IdChat equals u.IdChat into up
                        from u in up.DefaultIfEmpty()
                        select new ChatModel()
                        {
                            IdChat = p.IdChat,
                            Name = u.ChatName,
                            Desc = u.ChatDescription,
                            MainBackground = u.Background,
                            ProfileBackground = u.Avatar,
                            MembersCount = _db.ChatsUsersChatRoles.Count(x => x.IdChat == p.IdChat),
                            OwnerLogin = p.OwnerLogin,
                            IdPlanet = PlanetId,
                        };
            IList<ChatModel> list = await chats.AsNoTracking().ToListAsync();
            return PartialView(list);
        }

        public async Task<IActionResult> CreateChat()
        {
            var result = HttpContext.Session.Get<ChatCreate>("CreateChat");
            if (result != null)
            {
                return View(result);
            }
            ChatCreate edit = new ChatCreate();
            edit.Name = "Новый чат";
            HttpContext.Session.Set<ChatCreate>("CreateChat", edit);
            return View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> EditExistChat(string Name, string Desc)
        {
            int IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            ChatModel? chat = HttpContext.Session.Get<ChatModel>("Chat");

            if (chat == null)
            {
                return NotFound();
            }

            ChatsInfo? chatInfo = await _db.ChatsInfos.FirstOrDefaultAsync(x => x.IdChat == chat.IdChat);
            chatInfo.ChatName = Name ?? chat.Name;
            chatInfo.ChatDescription = Desc ?? chat.Desc;

            ChatCreate? create = HttpContext.Session.Get<ChatCreate>("EditChat");
            if (create?.MainBackground != null) chatInfo.Background = FileServices.CreateFileFromByteArray(_webHostEnvironment, create.MainBackground, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Chats\\Main", $"{chat.IdChat}_{IdPlanet}.jpg"));
            else chatInfo.Background = "\\img\\background\\MainBackground.jpg";
            if (create?.ProfileBack != null) chatInfo.Avatar = FileServices.CreateFileFromByteArray(_webHostEnvironment, create.MainBackground, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Chats\\Profile", $"{chat.IdChat}_{IdPlanet}.jpg"));
            else chatInfo.Avatar = "\\img\\background\\secondBack.jpg";

            HttpContext.Session.Remove("EditChat");
            _db.ChatsInfos.Update(chatInfo);
            await _db.SaveChangesAsync();

            return Redirect("/Chat/ChatList");

        }


        public async Task<IActionResult> RegChat(string Name, string Desc)
        {
            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            User? user = await _db.Users.FirstOrDefaultAsync(x => x.UserLogin == userModel.Login);
            int? IdPlanet = HttpContext.Session.GetInt("PlanetId");
            if (IdPlanet == null || user == null)
            {
                return NotFound();
            }
            Chat chat = new Chat();
            chat.IdPlanet = IdPlanet;
            chat.OwnerLogin = user.UserLogin;

            _db.Chats.Add(chat);
            await _db.SaveChangesAsync();

            ChatsInfo chatInfo = new ChatsInfo();
            chatInfo.IdChatNavigation = chat;
            chatInfo.ChatName = Name ?? "Новый чат";
            chatInfo.ChatDescription = Desc ?? "";

            ChatCreate? create = HttpContext.Session.Get<ChatCreate>("CreateChat");
            if(create?.MainBackground != null) chatInfo.Background = FileServices.CreateFileFromByteArray(_webHostEnvironment, create.MainBackground, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Chats\\Main", $"{chat.IdChat}_{IdPlanet}.jpg"));
            else chatInfo.Background = "\\img\\background\\MainBackground.jpg";
            if(create?.ProfileBack != null) chatInfo.Avatar = FileServices.CreateFileFromByteArray(_webHostEnvironment, create.ProfileBack, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Chats\\Profile", $"{chat.IdChat}_{IdPlanet}.jpg"));
            else chatInfo.Avatar = "\\img\\background\\secondBack.jpg";

            ChatsUsersChatRole chatRole = new ChatsUsersChatRole();
            chatRole.IdChat = chat.IdChat;
            chatRole.LoginUsers = user.UserLogin;
            chatRole.IdChatRole = 1; // 1 - Владелец, 2 - Админ, 3 - Модер, 4 - Пользователь, 5 - Заблокирован
            HttpContext.Session.Remove("CreateChat");
            _db.ChatsInfos.Add(chatInfo);
            _db.ChatsUsersChatRoles.Add(chatRole);
            await _db.SaveChangesAsync();

            return Redirect("/Chat/ChatList");

        }


        [HttpPost]
        public async Task<IActionResult> EditMain([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<ChatCreate>("EditChat");
            model.MainBackground = avatar;
            HttpContext.Session.Remove("EditChat");
            HttpContext.Session.Set<ChatCreate>("EditChat", model);
            return View("EditChat", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<ChatCreate>("EditChat");
            model.ProfileBack = avatar;
            HttpContext.Session.Remove("EditChat");
            HttpContext.Session.Set<ChatCreate>("EditChat", model);
            return View("EditChat", model);
        }

        public async Task<IActionResult> UploadMain([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<ChatCreate>("CreateChat");
            model.MainBackground = avatar;
            HttpContext.Session.Remove("CreateChat");
            HttpContext.Session.Set<ChatCreate>("CreateChat", model);
            return View("CreateChat", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfile([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<ChatCreate>("CreateChat");
            model.ProfileBack = avatar;
            HttpContext.Session.Remove("CreateChat");
            HttpContext.Session.Set<ChatCreate>("CreateChat", model);
            return View("CreateChat", model);
        }

        [HttpGet]
        public async Task<string> GetRole(PlanetUsersInfo userModel, int? IdPlanet)
        {

            if (userModel == null)
            {
                return "";
            }

            var roleId = _db.PlanetPlanetRoleUsers.FirstOrDefault(x => x.UserLogin == userModel.UserLogin && x.IdPlanet == IdPlanet)?.IdRole;
            if (roleId == null)
            {
                return "";
            }

            var roleName = _db.PlanetRoles.FirstOrDefault(x => x.IdRole == roleId)?.RoleName;
            if (roleName == null)
            {
                return "";
            }

            return roleName;
        }

    }
}
