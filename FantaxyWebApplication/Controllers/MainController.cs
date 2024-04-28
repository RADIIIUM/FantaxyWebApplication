﻿using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
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
            IdPlanet = await GetIdPlanet(IdPlanet);
            HttpContext.Session.Set("PlanetId", IdPlanet);

            var cookie = HttpContext.Request.Cookies[$"Planet_{IdPlanet}"];
            if (cookie != null)
            {
                UserModel? userModel = new UserModel();
                var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
                if (json == null)
                {
                    PlanetUsersInfo? glu = await _db.PlanetUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == IdPlanet);
                    if(glu == null)
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
                    var serialize = JsonSerializer.Serialize<UserModel>(userModel);
                    HttpContext.Response.Cookies.Append($"Profile_{IdPlanet}", serialize);

                }
                HttpContext.Session.Set("Access", userModel.Role);
                PlanetInfo js = JsonSerializer.Deserialize<PlanetInfo>(cookie);
                return Redirect("/Planet/MainPage");
            }
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
                    var serialize = JsonSerializer.Serialize<UserModel>(userModel);
                    HttpContext.Response.Cookies.Append($"Profile_{IdPlanet}", serialize);

                }
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

        public async Task<int?> GetIdPlanet(int? IdPlanet)
        {
            int? id = HttpContext.Session.GetInt("PlanetId");
            if (id == -1)
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

            IList<SearchModel> list = await planetsIQ.AsNoTracking().ToListAsync();
            return list;
        }

        private async Task<IList<SearchModel>> GetUserAsync(string search)
        {
            IQueryable<SearchModel> usersIQ = from p in _db.GlobalUsersInfos
                                                join s in _db.GlobalRoleUsers on p.UserLogin equals s.UserLogin into ps
                                                from s in ps.DefaultIfEmpty()
                                                select new SearchModel
                                                {
                                                    Id = p.UserLogin,
                                                    Name = p.UserName,
                                                    RoleOrStatus = s.IdRole ?? 4,
                                                    Avatar = p.Avatar,
                                                    Profile = p.ProfileBackground
                                                };
            if (!String.IsNullOrEmpty(search))
            {
                usersIQ = usersIQ.Where(s => s.Name.ToUpper().Contains(search.ToUpper()));
            }

            IList<SearchModel> list = await usersIQ.AsNoTracking().ToListAsync();
            return list;
        }
    }
}
