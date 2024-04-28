using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Models;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        public IActionResult Chat(int IdChat)
        {
            int PlanetId = HttpContext.Session.Get<int>("PlanetId");
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
            if(chat != null) {
                HttpContext.Session.Set<ChatModel>("Chat", chat);
                return View(chat);
            }
            else
            {
                return View("ChatList");
            }

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
                                Owner = _db.PlanetUsersInfos.FirstOrDefault(x => x.UserLogin == p.LoginOwner).UserName,
                                Avatar = _db.PlanetUsersInfos.FirstOrDefault(x => x.UserLogin == p.LoginOwner).Avatar,
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
                    return Json(new { success = true, ownerAvatar = userModel.Avatar, ownerName = userModel.Name });
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
            if(create?.ProfileBack != null) chatInfo.Avatar = FileServices.CreateFileFromByteArray(_webHostEnvironment, create.MainBackground, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Chats\\Profile", $"{chat.IdChat}_{IdPlanet}.jpg"));
            else chatInfo.Avatar = "\\img\\background\\secondBack.jpg";

            ChatsUsersChatRole chatRole = new ChatsUsersChatRole();
            chatRole.IdChatNavigation = chat;
            chatRole.LoginUsers = user.UserLogin;
            chatRole.IdChatRole = 1; // 1 - Владелец, 2 - Админ, 3 - Модер, 4 - Пользователь, 5 - Заблокирован
            HttpContext.Session.Remove("CreateChat");
            _db.ChatsInfos.Add(chatInfo);
            _db.ChatsUsersChatRoles.Add(chatRole);
            await _db.SaveChangesAsync();

            return Redirect("/Chat/ChatList");
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
