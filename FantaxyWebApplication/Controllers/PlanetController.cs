using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;

namespace FantaxyWebApplication.Controllers
{
    public class PlanetController : Controller
    {

        FantaxyContext _db;
        IWebHostEnvironment _appEnvironment;

        public PlanetController(FantaxyContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Posts()
        {
            return View();
        }
        public async Task<IActionResult> PostList(int IdPlanet)
        {
            IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            var posts = _db.Posts.Where(p => p.IdPlanet == IdPlanet).Select(p => new PostModel()
            {
                IdPost = p.IdPost,
                Title = p.PostsInfo.Title,
                Description = p.PostsInfo.PostText,
                Files = _db.PostFiles.Where(x => x.IdPost == p.IdPost).Select(x => x.PathFile).ToList<string>(),
                authorInfo = _db.PlanetUsersInfos.FirstOrDefault(x => x.IdPlanet == IdPlanet && x.UserLogin == p.OwnerLogin),
                LikeCount = _db.LikeDislikePosts.Count(y => y.IdPost == p.IdPost && y.LikeOrDislike == true),
                DislikeCount = _db.LikeDislikePosts.Count(y => y.IdPost == p.IdPost && y.LikeOrDislike == false),
                IsLiked = _db.LikeDislikePosts.FirstOrDefault(x => x.IdPost == p.IdPost && userModel.Login == x.UserLogin).LikeOrDislike,
            });

            IList<PostModel> list = await posts.AsNoTracking().ToListAsync();
            return PartialView(list);
        }
        [HttpGet]
        public async Task<IActionResult> ConcretPost(int IdPlanet)
        {
            IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            var posts = _db.Posts.Where(p => p.IdPlanet == IdPlanet && p.OwnerLogin == userModel.Login).Select(p => new PostModel()
            {
                IdPost = p.IdPost,
                Title = p.PostsInfo.Title,
                Description = p.PostsInfo.PostText,
                Files = _db.PostFiles.Where(x => x.IdPost == p.IdPost).Select(x => x.PathFile).ToList<string>(),
                authorInfo = _db.PlanetUsersInfos.FirstOrDefault(x => x.IdPlanet == IdPlanet && x.UserLogin == p.OwnerLogin),
                LikeCount = _db.LikeDislikePosts.Count(y => y.IdPost == p.IdPost && y.LikeOrDislike == true),
                DislikeCount = _db.LikeDislikePosts.Count(y => y.IdPost == p.IdPost && y.LikeOrDislike == false),
                IsLiked = _db.LikeDislikePosts.FirstOrDefault(x => x.IdPost == p.IdPost && userModel.Login == x.UserLogin).LikeOrDislike,
            });

            IList<PostModel> list = await posts.AsNoTracking().ToListAsync();
            return PartialView(list);
        }

        public async Task<int?> GetIdPlanet(int? IdPlanet)
        {
            int? id = HttpContext.Session.GetInt("PlanetId");
            if(id == -1)
            {
                HttpContext.Session.Set("PlanetId", IdPlanet);
                id = IdPlanet;
                return id;
            }
            else
            {
                return id;
            }
        }
        public async Task<IActionResult> MainPage()
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var cookie = HttpContext.Request.Cookies[$"Planet_{IdPlanet}"];
            PlanetInfo? plInfo = JsonSerializer.Deserialize<PlanetInfo>(cookie) ?? null;
            if (plInfo == null)
            {
                return NotFound();
            }
            else
            {
                var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
                UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

                if (userModel != null)
                {
                    if(userModel.Role != "Заблокирован")
                    {
                        return View(plInfo);
                    }
                    return Redirect("/Main/Planets");

                }
                else
                {
                    return NotFound();
                }
            }

        }
        public IActionResult Users()
        {
            return View();
        }

        public async Task<IActionResult> UserList(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetUserAsync(""));
            }
            return PartialView(await GetUserAsync(search));
        }

        private async Task<IList<SearchModel>> GetUserAsync(string search)
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var usersIQ = _db.PlanetUsersInfos.Where(x => x.IdPlanet == IdPlanet).Select(x => new SearchModel
            {
                Id = x.UserLogin,
                Name = x.UserName,
                RoleOrStatus = _db.PlanetPlanetRoleUsers.Where(y => y.IdPlanet == IdPlanet).FirstOrDefault(y => y.UserLogin == x.UserLogin).IdRole?? 4,
                Avatar = x.Avatar,
                Profile = x.ProfileBackground
            });
           
            usersIQ = usersIQ.Where(s => s.RoleOrStatus != 5);

            if (!String.IsNullOrEmpty(search))
            {
                usersIQ = usersIQ.Where(s => s.Name.ToUpper().Contains(search.ToUpper()));
            }

            IList<SearchModel> list = await usersIQ.AsNoTracking().ToListAsync();
            return list;
        }
        /* 
        РЕДАКТИРОВАНИЕ ПЛАНЕТЫ
        РЕДАКТИРОВАНИЕ ПЛАНЕТЫ
        РЕДАКТИРОВАНИЕ ПЛАНЕТЫ
        */

        public async Task<IActionResult> EditPlanet()
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var cookie = HttpContext.Request.Cookies[$"Planet_{IdPlanet}"];
            PlanetInfo? plInfo = JsonSerializer.Deserialize<PlanetInfo>(cookie) ?? null;
            if (plInfo == null)
            {
                return NotFound();
            }

            var result = HttpContext.Session.Get<CreatePlanetModel>("EditPlanet");
            if (result != null)
            {
                return View(result);
            }
            CreatePlanetModel edit = new CreatePlanetModel();
            edit.Name = plInfo.PlanetName;
            edit.Description = plInfo.PlanetDescription;
            edit.Avatar = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, plInfo.Avatar));
            edit.Profile = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, plInfo.ProfileBackground));
            edit.Main = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, plInfo.MainBackground));

            HttpContext.Session.Set<CreatePlanetModel>("EditPlanet", edit);
            return View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlanet(string Name, string Desc)
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var result = HttpContext.Session.Get<CreatePlanetModel>("EditPlanet");
            PlanetInfo? planetInfo = await _db.PlanetInfos.FirstOrDefaultAsync(x => x.IdPlanet == IdPlanet);

            if (result == null || planetInfo == null)
            {
                return NotFound();
            }

            planetInfo.PlanetName = Name ?? planetInfo.PlanetName;
            planetInfo.PlanetDescription = Desc ?? planetInfo.PlanetDescription;

            if (result?.Avatar != null) planetInfo.Avatar = FileServices.CreateFileFromByteArray(_appEnvironment, result.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Avatar", $"{planetInfo.IdPlanet}.jpg"));
            if (result?.Main != null) planetInfo.MainBackground = FileServices.CreateFileFromByteArray(_appEnvironment, result.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Main", $"{planetInfo.IdPlanet}.jpg"));
            if (result?.Profile != null) planetInfo.ProfileBackground = FileServices.CreateFileFromByteArray(_appEnvironment, result.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Profile", $"{planetInfo.IdPlanet}.jpg"));

            _db.PlanetInfos.Update(planetInfo);
            await _db.SaveChangesAsync();

            var json = JsonSerializer.Serialize<PlanetInfo>(planetInfo);
            HttpContext.Response.Cookies.Append($"Planet_{IdPlanet}", json);

                return View("MainPage", planetInfo);
        }

        public async Task<IActionResult> EditAvatar([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<CreatePlanetModel>("EditPlanet");
            model.Avatar = avatar;
            HttpContext.Session.Remove("EditPlanet");
            HttpContext.Session.Set<CreatePlanetModel>("EditPlanet", model);
            return View("EditPlanet", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditMain([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<CreatePlanetModel>("EditPlanet");
            model.Main = avatar;
            HttpContext.Session.Remove("EditPlanet");
            HttpContext.Session.Set<CreatePlanetModel>("EditPlanet", model);
            return View("EditPlanet", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<CreatePlanetModel>("EditPlanet");
            model.Profile = avatar;
            HttpContext.Session.Remove("EditPlanet");
            HttpContext.Session.Set<CreatePlanetModel>("EditPlanet", model);
            return View("EditPlanet", model);
        }




        /* 
        СОЗДАНИЕ ПЛАНЕТЫ
        СОЗДАНИЕ ПЛАНЕТЫ
        СОЗДАНИЕ ПЛАНЕТЫ
        */
        public async Task<IActionResult> CreatePlanet()
        {
            var result = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
            if (result != null)
            {
                HttpContext.Session.Remove("CreatePlanet");
            }
            CreatePlanetModel edit = new CreatePlanetModel();
            HttpContext.Session.Set<CreatePlanetModel>("CreatePlanet", edit);
            return View(edit);
        }

        public async Task<User?> SearchCurator(string userLogin)
        {
            var moderators = from user in _db.Users
                        join role in _db.GlobalRoleUsers on user.UserLogin equals role.UserLogin
                        where role.IdRole == 3 && role.UserLogin != userLogin
                        select user;

            if (moderators.Count() == 0)
            {
                throw new Exception("No moderators found.");
            }
            User curator = moderators.OrderBy(m => m.PlanetCuratorLoginNavigations.Count).FirstOrDefault();

            if (curator == null || curator.PlanetCuratorLoginNavigations.Count >= 21)
            {
                var administrators = from user in _db.Users
                                     join role in _db.GlobalRoleUsers on user.UserLogin equals role.UserLogin
                                     where role.IdRole == 2 && role.UserLogin != userLogin
                                     select user;

                if (administrators.Count() == 0) throw new Exception("No moderators or administrators found.");
                else curator = administrators.OrderBy(a => a.PlanetCuratorLoginNavigations.Count).FirstOrDefault();
                return curator;
            }
            return curator;
        }

        [HttpPost]
        public async Task<IActionResult> RegPlanet(string Name, string Desc)
        {
            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            Planet planet = new Planet();
            planet.OwnerLogin = userModel.Login;
            planet.CuratorLoginNavigation = await SearchCurator(userModel.Login);

            _db.Planets.Add(planet);
            await _db.SaveChangesAsync();

            PlanetUser pu = new PlanetUser();
            pu.UserLogin = userModel.Login;
            pu.IdPlanetNavigation = planet;

            PlanetInfo planetInfo = new PlanetInfo();
            planetInfo.IdPlanetNavigation = planet;
            planetInfo.PlanetName = string.IsNullOrEmpty(Name) ? "Новая планета" : Name;
            planetInfo.PlanetDescription = Desc ?? "Новая планета на просторах Fantaxy!";

            int lastId = _db.Planets.OrderByDescending(p => p.IdPlanet).FirstOrDefault()?.IdPlanet ?? 0;
            int newId = lastId + 1;

            CreatePlanetModel? create = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
            if (create?.Avatar != null) planetInfo.Avatar = FileServices.CreateFileFromByteArray(_appEnvironment, create.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Avatar", $"{planet.IdPlanet}.jpg"));
            else planetInfo.Avatar = "\\img\\icon\\Planet.png";

            if (create?.Main != null) planetInfo.MainBackground = FileServices.CreateFileFromByteArray(_appEnvironment, create.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Main", $"{planet.IdPlanet}.jpg"));
            else planetInfo.MainBackground = "\\img\\background\\MainBackground.jpg";

            if (create?.Profile != null) planetInfo.ProfileBackground = FileServices.CreateFileFromByteArray(_appEnvironment, create.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Profile", $"{planet.IdPlanet}.jpg"));
            else planetInfo.ProfileBackground = "\\img\\background\\secondBack.jpg";
            PlanetUsersInfo pui = new PlanetUsersInfo();
            pui.UserLogin = userModel.Login;
            pui.UserName = userModel.Name;
            pui.UserDescription = userModel.Description ?? "Hello world!";
            pui.Avatar = userModel.Avatar;
            pui.ProfileBackground = userModel.Profile;
            pui.MainBackground = userModel.Main;
            pui.IdPlanet = newId;

            StatusesPlanet sp = new StatusesPlanet();
            sp.IdPlanet = newId;
            sp.IdPlanetNavigation = planet;
            sp.IdStatus = 1;

            PlanetPlanetRoleUser planetPlanetRoleUser = new PlanetPlanetRoleUser();
            planetPlanetRoleUser.IdPlanetNavigation = planet;
            planetPlanetRoleUser.UserLogin = userModel.Login;
            planetPlanetRoleUser.IdRole = 1;

            /* Для куратора */
            GlobalUsersInfo? curator = await _db.GlobalUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == planet.CuratorLogin);
            PlanetUsersInfo? curatorProfile = new PlanetUsersInfo();
            curatorProfile.UserLogin = curator.UserLogin;
            curatorProfile.UserName = curator.UserName;
            curatorProfile.UserDescription = curator.UserDescription ?? "Hello world!";
            curatorProfile.Avatar = curator.Avatar;
            curatorProfile.ProfileBackground = curator.ProfileBackground;
            curatorProfile.MainBackground = curator.MainBackground;
            curatorProfile.IdPlanet = planet.IdPlanet;

            PlanetPlanetRoleUser curatorRole = new PlanetPlanetRoleUser();
            curatorRole.IdPlanetNavigation = planet;
            curatorRole.UserLogin = curatorProfile.UserLogin;
            curatorRole.IdRole = 6; // Куратор


            _db.PlanetUsers.Add(pu);
            _db.PlanetUsersInfos.Add(pui);
            _db.StatusesPlanets.Add(sp);
            _db.PlanetInfos.Add(planetInfo);
            _db.PlanetUsersInfos.Add(curatorProfile);
            _db.PlanetPlanetRoleUsers.Add(planetPlanetRoleUser);
            _db.PlanetPlanetRoleUsers.Add(curatorRole);

            await _db.SaveChangesAsync();
            PlanetGenerate planetGenerate = new PlanetGenerate(_db);
            planetGenerate.RegenerateCookiePlanet(new HttpContextAccessor(), userModel);

            return Redirect("/Main/Planets");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
            model.Avatar = avatar;
            HttpContext.Session.Remove("CreatePlanet");
            HttpContext.Session.Set<CreatePlanetModel>("CreatePlanet", model);
            return View("CreatePlanet", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMain([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
            model.Main = avatar;
            HttpContext.Session.Remove("CreatePlanet");
            HttpContext.Session.Set<CreatePlanetModel>("CreatePlanet", model);
            return View("CreatePlanet", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfile([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            var model = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
            model.Profile = avatar;
            HttpContext.Session.Remove("CreatePlanet");
            HttpContext.Session.Set<CreatePlanetModel>("CreatePlanet", model);
            return View("CreatePlanet", model);
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
