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
            var posts = from p in _db.Posts where p.IdPlanet == IdPlanet
                                          join u in _db.PlanetUsersInfos on p.OwnerLogin equals u.UserLogin into up
                                          from u in up.DefaultIfEmpty()
                                          select new PostModel()
                                          {
                                              IdPost = p.IdPost,
                                              Title = p.PostsInfo.Title,
                                              Description = p.PostsInfo.PostText,
                                              Files = _db.PostFiles.Where(x => x.IdPost == p.IdPost).Select(x => x.PathFile).ToList<string>(),
                                              authorInfo = u,
                                              LikeCount = _db.LikesPosts.Count(y => y.IdPost == p.IdPost),
                                              DislikeCount = _db.DisikesPosts.Count(y => y.IdPost == p.IdPost),
                                          };

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
                    return View(plInfo);
                }
                else
                {
                    return NotFound();
                }
            }

            //var usInfo = HttpContext.Request.Cookies["UserInfo"];
            //UserModel? user = JsonSerializer.Deserialize<UserModel>(usInfo);
            //IdPlanet = await GetIdPlanet(IdPlanet);

            //HttpContext.Session.Set("PlanetId", IdPlanet);
            //var cookie = HttpContext.Request.Cookies[$"Planet_{IdPlanet}"];
            //if (cookie != null)
            //{
            //    UserModel? userModel = new UserModel();
            //    var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            //    if (json == null)
            //    {
            //        PlanetUsersInfo glu = await _db.PlanetUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == IdPlanet);
            //        userModel.Name = glu.UserName;
            //        userModel.Avatar = glu.Avatar;
            //        userModel.Main = glu.MainBackground;
            //        userModel.Profile = glu.ProfileBackground;
            //        userModel.Description = glu.UserDescription;
            //        userModel.Login = glu.UserLogin;
            //        userModel.Role = await GetRole(glu, IdPlanet);
            //        var serialize = JsonSerializer.Serialize<UserModel>(userModel);
            //        HttpContext.Response.Cookies.Append($"Profile_{IdPlanet}", serialize);

            //    }
            //    HttpContext.Session.Set("Access", userModel.Role);
            //    PlanetInfo js = JsonSerializer.Deserialize<PlanetInfo>(cookie);
            //    return View(js);
            //}
            //PlanetInfo? plInfo = await _db.PlanetInfos.FirstOrDefaultAsync(x => x.IdPlanet == IdPlanet);
            //if (plInfo != null)
            //{
            //    var json = JsonSerializer.Serialize<PlanetInfo>(plInfo);
            //    HttpContext.Response.Cookies.Append($"Planet_{IdPlanet}", json);
            //    json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            //    UserModel? userModel = new UserModel();
            //    if (json == null)
            //    {
            //        PlanetUsersInfo glu = await _db.PlanetUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == IdPlanet);
            //        userModel.Name = glu.UserName;
            //        userModel.Avatar = glu.Avatar;
            //        userModel.Main = glu.MainBackground;
            //        userModel.Profile = glu.ProfileBackground;
            //        userModel.Description = glu.UserDescription;
            //        userModel.Login = glu.UserLogin;
            //        userModel.Role = await GetRole(glu, IdPlanet);
            //        var serialize = JsonSerializer.Serialize<UserModel>(userModel);
            //        HttpContext.Response.Cookies.Append($"Profile_{IdPlanet}", serialize);
            //    }
            //    HttpContext.Session.Set("Access", userModel.Role);
            //    return View(plInfo);
            //}
            //return Redirect("/Main/Planets");

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

        public async Task<IActionResult> RegPlanet(string Name, string Desc)
        {
            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            User user = await _db.Users.FirstOrDefaultAsync(x => x.UserLogin == userModel.Login);

            Planet planet = new Planet();
            planet.OwnerLoginNavigation = user;
            planet.CuratorLoginNavigation = await SearchCurator(userModel.Login);

            PlanetUser pu = new PlanetUser();
            pu.UserLoginNavigation = user;
            pu.IdPlanetNavigation = planet;

            PlanetInfo planetInfo = new PlanetInfo();
            planetInfo.IdPlanetNavigation = planet;
            planetInfo.PlanetName = Name ?? "Новая планета";
            planetInfo.PlanetDescription = Desc ?? "Новая планета на просторах Fantaxy!";

            int lastId = _db.Planets.OrderByDescending(p => p.IdPlanet).FirstOrDefault()?.IdPlanet ?? 0;
            int newId = lastId + 1;

            CreatePlanetModel? create = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
                if (create?.Avatar != null) planetInfo.Avatar = FileServices.CreateFileFromByteArray(_appEnvironment, create.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Avatar", $"{newId}.jpg"));
                else planetInfo.Avatar = "\\img\\icon\\Planet.png";
                
                if (create?.Main != null) planetInfo.MainBackground = FileServices.CreateFileFromByteArray(_appEnvironment,create.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Main", $"{newId}.jpg"));
                else planetInfo.MainBackground = "\\img\\background\\MainBackground.jpg";
                
                if (create?.Profile != null) planetInfo.ProfileBackground = FileServices.CreateFileFromByteArray(_appEnvironment,create.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Profile", $"{newId}.jpg"));
                else planetInfo.MainBackground = "\\img\\background\\secondBack.jpg";
            PlanetUsersInfo pui = new PlanetUsersInfo();
            pui.UserLoginNavigation = user;
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

            PlanetPlanetRoleUser ppru = new PlanetPlanetRoleUser();
            ppru.IdPlanetNavigation = planet;
            ppru.UserLoginNavigation = user;
            ppru.IdRole = 1;

            _db.Planets.Add(planet);
            _db.PlanetUsers.Add(pu);
            _db.PlanetUsersInfos.Add(pui);
            _db.StatusesPlanets.Add(sp);
            _db.PlanetInfos.Add(planetInfo);
            _db.PlanetPlanetRoleUsers.Add(ppru);

            await _db.SaveChangesAsync();

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
