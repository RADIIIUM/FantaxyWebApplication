using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantaxyWebApplication.Services
{
    public class PlanetGenerate
    {
        FantaxyContext _db;
        public PlanetGenerate (FantaxyContext context)
        {
            _db = context;
        }
        //public async void RegenerateCookiePlanet(IWebHostEnvironment webHost, string UserLogin)
        //{
        //    GlobalUsersInfo glu = await _db.GlobalUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == UserLogin);
        //    UserModel us = new UserModel();

        //    us.Name = glu.UserName;
        //    us.Avatar = glu.Avatar;
        //    us.Main = glu.MainBackground;
        //    us.Profile = glu.ProfileBackground;
        //    us.Description = glu.UserDescription;
        //    us.Login = glu.UserLogin;
        //    us.Email = glu.UserEmail;
        //    us.Telephone = glu.UserTelephone;
        //    us.Role = await GetRole(glu);
        //    var planetList = (from p in _db.PlanetUsersInfos
        //                      where p.UserLogin == user.UserLogin
        //                      join pln in _db.PlanetInfos on p.IdPlanet equals pln.IdPlanet into planet
        //                      from pln in planet.DefaultIfEmpty()
        //                      join s in _db.StatusesPlanets on p.IdPlanet equals s.IdPlanet into ps
        //                      from s in ps.DefaultIfEmpty()
        //                      where s.IdStatus != 3
        //                      select pln).ToList();
        //    us.planetList = planetList;


        //    var json = JsonSerializer.Serialize<UserModel>(us);
        //    HttpContext.Response.Cookies.Append("UserInfo", json);
        //}
    }
}
