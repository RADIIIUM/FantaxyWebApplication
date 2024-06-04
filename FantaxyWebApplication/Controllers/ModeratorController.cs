using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FantaxyWebApplication.Controllers
{
    public class ModeratorController : Controller
    {
        FantaxyContext _db;
        public ModeratorController(FantaxyContext db) 
        {
            _db = db;
        }

        [HttpGet]
        public async Task<JsonResult> GetAccess()
        {
            var usInfo = HttpContext.Request.Cookies["UserInfo"];
            UserModel? user = JsonSerializer.Deserialize<UserModel>(usInfo);

            if (user != null)
            {
                int? UserRole = await (from p in _db.GlobalRoleUsers
                                   where p.UserLogin == user.Login
                                   select p.IdRole).FirstOrDefaultAsync();
                if (UserRole <= 3 && UserRole > 0)
                {
                    return Json(new { success = true, role = UserRole});
                }
                else
                {
                    return Json(new { success = false});
                }
            }
            else
            {
                return Json(new { success = false});
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPlanetAccess()
        {
            int? PlanetId = HttpContext.Session.GetInt("PlanetId");
            var usInfo = HttpContext.Request.Cookies[$"Profile_{PlanetId}"];
            UserModel? user = JsonSerializer.Deserialize<UserModel>(usInfo);
            if (user != null)
            {
                int? UserRole = await (from p in _db.PlanetPlanetRoleUsers
                                       where p.UserLogin == user.Login
                                       select p.IdRole).FirstOrDefaultAsync();
                if (UserRole <= 3 && UserRole > 0)
                {
                    return Json(new { success = true, role = UserRole });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        public async Task<JsonResult> ChangeRole ([FromBody] UserModel user)
        {
            GlobalRoleUser? userRole = await _db.GlobalRoleUsers.FirstOrDefaultAsync(x => x.UserLogin == user.Login);
            if (userRole != null)
            {
                int? IdRole = _db.GlobalRoles.FirstOrDefault(x => x.RoleName.Contains(user.Role) || x.RoleName == user.Role).IdRole;
                userRole.IdRole = IdRole ?? 4;
                _db.GlobalRoleUsers.Update(userRole);
                await _db.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UserBan ([FromBody] ModeratorModel user)
        {
            GlobalRoleUser? userRole = await _db.GlobalRoleUsers.FirstOrDefaultAsync(x => x.UserLogin == user.Login);
            if(userRole != null)
            {
                userRole.IdRole = 5;
                _db.GlobalRoleUsers.Update(userRole);
                await _db.SaveChangesAsync();

                return Json(new { success =  true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UserUnban([FromBody] ModeratorModel user)
        {
            GlobalRoleUser? userRole = await _db.GlobalRoleUsers.FirstOrDefaultAsync(x => x.UserLogin == user.Login);
            if (userRole != null)
            {
                userRole.IdRole = 4;
                _db.GlobalRoleUsers.Update(userRole);
                await _db.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        public async Task<JsonResult> ChangePlanetRole([FromBody] UserModel user)
        {
            int? PlanetId = HttpContext.Session.GetInt("PlanetId");
            PlanetPlanetRoleUser? userRole = await _db.PlanetPlanetRoleUsers.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == PlanetId);
            if (userRole != null)
            {
                int? IdRole = _db.PlanetRoles.FirstOrDefault(x => x.RoleName.Contains(user.Role) || x.RoleName == user.Role).IdRole;
                userRole.IdRole = IdRole ?? 4;
                _db.PlanetPlanetRoleUsers.Update(userRole);
                await _db.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> PlanetUserBan([FromBody] ModeratorModel user)
        {
            int? PlanetId = HttpContext.Session.GetInt("PlanetId");
            PlanetPlanetRoleUser? userRole = await _db.PlanetPlanetRoleUsers.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == PlanetId);
            if (userRole != null)
            {
                userRole.IdRole = 5;
                _db.PlanetPlanetRoleUsers.Update(userRole);
                await _db.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> PlanetUserUnban([FromBody] ModeratorModel user)
        {
            int? PlanetId = HttpContext.Session.GetInt("PlanetId");
            PlanetPlanetRoleUser? userRole = await _db.PlanetPlanetRoleUsers.FirstOrDefaultAsync(x => x.UserLogin == user.Login && x.IdPlanet == PlanetId);
            if (userRole != null)
            {
                userRole.IdRole = 4;
                _db.PlanetPlanetRoleUsers.Update(userRole);
                await _db.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }



        [HttpPost]
        public async Task<JsonResult> PlanetFreeze ()
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            StatusesPlanet? planetStatus = await _db.StatusesPlanets.FirstOrDefaultAsync(x => x.IdPlanet == IdPlanet);
            if (planetStatus != null)
            {
                if(planetStatus.IdStatus == 3)
                {
                    planetStatus.IdStatus = 1;
                    _db.StatusesPlanets.Update(planetStatus);
                }
                else
                {
                    planetStatus.IdStatus = 3;
                    _db.StatusesPlanets.Update(planetStatus);
                }
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> BanPlanet()
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            Planet planet = _db.Planets.FirstOrDefault(x => x.IdPlanet == IdPlanet);
            StatusesPlanet? planetStatus = await _db.StatusesPlanets.FirstOrDefaultAsync(x => x.IdPlanet == IdPlanet);
            if(planetStatus.IdStatus != 3)
            {
                return Json(new { success = false, reason = "Планета должна быть сначала заморожена" });
            }
            _db.Planets.Remove(planet);
            await _db.SaveChangesAsync();

            return Json( new { success = true });

        }

        public async Task<IActionResult> FreezePlanets()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FreezePlanetsList(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetFreezePlanetAsync(""));
            }
            return PartialView(await GetFreezePlanetAsync(search));
        }

        public async Task<IActionResult> BannedPlanetUser()
        {
            return View();
        }


        public async Task<IActionResult> BannedUser()
        {
            return View();
        }

        public async Task<IActionResult> BannedUserList(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetBannedUserAsync(""));
            }
            return PartialView(await GetBannedUserAsync(search));
        }

        public async Task<IActionResult> BannedPlanetUserList(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetBannedPlanetUserAsync(""));
            }
            return PartialView(await GetBannedPlanetUserAsync(search));
        }

        private async Task<IList<SearchModel>> GetBannedPlanetUserAsync(string search)
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var usersIQ = _db.PlanetUsersInfos.Where(x => x.IdPlanet == IdPlanet).Select(x => new SearchModel
            {
                Id = x.UserLogin,
                Name = x.UserName,
                RoleOrStatus = _db.PlanetPlanetRoleUsers.OrderBy(y => y.IdRole).FirstOrDefault(y => y.UserLogin == x.UserLogin).IdRole ?? 5,
                Avatar = x.Avatar,
                Profile = x.ProfileBackground
            });

            usersIQ = usersIQ.Where(s => s.RoleOrStatus == 5);

            if (!String.IsNullOrEmpty(search))
            {
                usersIQ = usersIQ.Where(s => s.Name.ToUpper().Contains(search.ToUpper()));
            }

            IList<SearchModel> list = await usersIQ.AsNoTracking().ToListAsync();
            return list;
        }

        private async Task<IList<SearchModel>> GetFreezePlanetAsync(string search)
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

            IList<SearchModel> list = await planetsIQ.Where(x => x.RoleOrStatus == 3).AsNoTracking().ToListAsync();
            return list;
        }

        private async Task<IList<SearchModel>> GetBannedUserAsync(string search)
        {
            var usersIQ = _db.GlobalUsersInfos.Join(_db.GlobalRoleUsers, x => x.UserLogin,
                y => y.UserLogin, (x, y) => new SearchModel
                {
                    Id = x.UserLogin,
                    Name = x.UserName,
                    RoleOrStatus = y.IdRole ?? 4,
                    Avatar = x.Avatar,
                    Profile = x.ProfileBackground
                });
            usersIQ = usersIQ.Where(s => s.RoleOrStatus == 5);

            if (!String.IsNullOrEmpty(search))
            {
                usersIQ = usersIQ.Where(s => s.Name.ToUpper().Contains(search.ToUpper()));
            }

            IList<SearchModel> list = await usersIQ.AsNoTracking().ToListAsync();
            return list;
        }

    }
}
