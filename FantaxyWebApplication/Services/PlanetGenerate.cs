using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FantaxyWebApplication.Services
{
    public class PlanetGenerate
    {
        FantaxyContext _db;
        public PlanetGenerate (FantaxyContext context)
        {
            _db = context;
        }
        public async void RegenerateCookiePlanet(IHttpContextAccessor access, UserModel model)
        {
            model.planetList = (from p in _db.PlanetUsersInfos
                                where p.UserLogin == model.Login
                                join pln in _db.PlanetInfos on p.IdPlanet equals pln.IdPlanet into planet
                                from pln in planet.DefaultIfEmpty()
                                join s in _db.StatusesPlanets on p.IdPlanet equals s.IdPlanet into ps
                                from s in ps.DefaultIfEmpty()
                                where s.IdStatus != 3
                                select pln).ToList();

            var json = JsonSerializer.Serialize<UserModel>(model);
            access.HttpContext.Response.Cookies.Append("UserInfo", json);
        }
    }
}
