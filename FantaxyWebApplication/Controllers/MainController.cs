using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace FantaxyWebApplication.Controllers
{
    public class MainController : Controller
    {
        private readonly FantaxyContext _db;
        public MainController(FantaxyContext db)
        {
            _db = db;
        }
        [Authorize]
        public IActionResult Users()
        {
            var json = HttpContext.Request.Cookies["UserInfo"];
            if(json == null)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public IActionResult Planets()
        {
            return View();
        }

        public async Task<PlanetUsersInfo?> CreateProfile(int? IdPlanet)
        {
            if(IdPlanet == null)
            {
                return null;
            }
            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            if(userModel == null)
            {
                return null;
            }
            User user = await _db.Users.FirstOrDefaultAsync(x => x.UserLogin == userModel.Login);

            PlanetUsersInfo pui = new PlanetUsersInfo();
                pui.UserLoginNavigation = user;
                pui.UserName = userModel.Name;
                pui.UserDescription = userModel.Description ?? "Hello world!";
                pui.Avatar = userModel.Avatar;
                pui.ProfileBackground = userModel.Profile;
                pui.MainBackground = userModel.Main;
                pui.IdPlanet = IdPlanet;

            PlanetPlanetRoleUser pl = new PlanetPlanetRoleUser();
            pl.IdPlanet = IdPlanet;
            pl.UserLoginNavigation = user;
            pl.IdRole = 4;

            PlanetUser pu = new PlanetUser();
            pu.UserLoginNavigation = user;
            pu.IdPlanet = IdPlanet;

            _db.PlanetUsersInfos.Add(pui);
            _db.PlanetPlanetRoleUsers.Add(pl);
            _db.PlanetUsers.Add(pu);
            await _db.SaveChangesAsync();
            return pui;
        }

        public async Task<IActionResult> RedirectToPlanetPage(int? IdPlanet)
        {
            var usInfo = HttpContext.Request.Cookies["UserInfo"];
            UserModel? user = JsonSerializer.Deserialize<UserModel>(usInfo);
            HttpContext.Session.Set("PlanetId", IdPlanet);
            PlanetInfo? plInfo = await _db.PlanetInfos.FirstOrDefaultAsync(x => x.IdPlanet == IdPlanet);
            if (plInfo != null)
            {
                var json = JsonSerializer.Serialize<PlanetInfo>(plInfo);
                HttpContext.Response.Cookies.Append($"Planet_{IdPlanet}", json);
                json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
                UserModel? userModel = new UserModel();
                if (json == null)
                {

                    PlanetUsersInfo? glu = await _db.PlanetUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == IdPlanet);
                    if (glu == null)
                    {
                        glu = await CreateProfile(IdPlanet);
                    }
                    userModel.Name = glu.UserName;
                    userModel.Avatar = glu.Avatar;
                    userModel.Main = glu.MainBackground;
                    userModel.Profile = glu.ProfileBackground;
                    userModel.Description = glu.UserDescription;
                    userModel.Login = glu.UserLogin;
                    userModel.Role = await GetRole(glu, IdPlanet);
                    if(userModel.Role == "Заблокирован") 
                    {
                        return View("Banned");
                    }
                    var serialize = JsonSerializer.Serialize<UserModel>(userModel);
                    HttpContext.Response.Cookies.Append($"Profile_{IdPlanet}", serialize);

                }
                PlanetGenerate planetGenerate = new PlanetGenerate(_db);
                planetGenerate.RegenerateCookiePlanet(new HttpContextAccessor(), user);
                HttpContext.Session.Set("Access", userModel.Role);
                return Redirect("/Planet/MainPage");
            }
            return Redirect("/Main/Planets");
        }


        public async Task<IActionResult> PlanetPartial(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetPlanetAsync(""));
            }
            return PartialView(await GetPlanetAsync(search));
        }

        public async Task<IActionResult> UsersPartial(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetUserAsync(""));
            }
            return PartialView(await GetUserAsync(search));
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

        private async Task<IList<SearchModel>> GetPlanetAsync(string search)
        {
            IQueryable<SearchModel> planetsIQ = from p in _db.PlanetInfos
                                                 join s in _db.StatusesPlanets on p.IdPlanet equals s.IdPlanet into ps
                                                 from s in ps.DefaultIfEmpty()
                                                 select new SearchModel()
                                                 {
                                                     Id = p.IdPlanet.ToString(),
                                                     Name = p.PlanetName,
                                                     RoleOrStatus = s.IdStatus ?? 1,
                                                     Avatar = p.Avatar,
                                                     Profile = p.ProfileBackground
                                                 };
            if (!String.IsNullOrEmpty(search))
            {
                planetsIQ = planetsIQ.Where(s => s.Name.Contains(search));
            }

            IList<SearchModel> list = await planetsIQ.Where(x => x.RoleOrStatus != 3).AsNoTracking().ToListAsync();
            return list;
        }

        private async Task<IList<SearchModel>> GetUserAsync(string search)
        {
            var usersIQ = _db.GlobalUsersInfos.Join(_db.GlobalRoleUsers, x => x.UserLogin,
                y => y.UserLogin, (x,y) => new SearchModel
                {
                    Id = x.UserLogin,
                    Name = x.UserName,
                    RoleOrStatus = y.IdRole ?? 4,
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
    }
}
