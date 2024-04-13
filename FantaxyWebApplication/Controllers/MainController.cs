using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public async Task<IActionResult> PlanetPartial(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(GetPlanetAsync(null));
            }
            return PartialView(GetPlanetAsync(search));
        }

        public async Task<IActionResult> UsersPartial(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView(await GetUserAsync(""));
            }
            return PartialView(await GetUserAsync(search));
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
