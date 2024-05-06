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
        public async Task<JsonResult> PlanetFreeze (int Id)
        {
            StatusesPlanet? planetStatus = await _db.StatusesPlanets.FirstOrDefaultAsync(x => x.IdPlanet == Id);
            if (planetStatus != null)
            {
                planetStatus.IdStatus = 3;
                _db.StatusesPlanets.Update(planetStatus);
                await _db.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
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
