using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            PlanetInfo planetInfo = new PlanetInfo();
            planetInfo.IdPlanetNavigation = planet;
            planetInfo.PlanetName = Name ?? "Новая планета";
            planetInfo.PlanetDescription = Desc ?? "Новая планета на просторах Fantaxy!";

            CreatePlanetModel create = HttpContext.Session.Get<CreatePlanetModel>("CreatePlanet");
            if (create != null)
            {
                if (create.Avatar != null) planetInfo.Avatar = FileServices.CreateFileFromByteArray(_appEnvironment,create.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Avatar", $"{planetInfo.IdPlanet}.jpg"));
                if (create.Main != null) planetInfo.MainBackground = FileServices.CreateFileFromByteArray(_appEnvironment,create.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Main", $"{planetInfo.IdPlanet}.jpg"));
                if (create.Profile != null) planetInfo.ProfileBackground = FileServices.CreateFileFromByteArray(_appEnvironment,create.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Planets\\Profile", $"{planetInfo.IdPlanet}.jpg"));
            }

            PlanetUsersInfo pui = new PlanetUsersInfo();
            pui.UserLoginNavigation = user;
            pui.UserName = userModel.Name;
            pui.UserDescription = userModel.Description ?? "Hello world!";
            pui.Avatar = userModel.Avatar;
            pui.ProfileBackground = userModel.Profile;
            pui.MainBackground = userModel.Main;
            pui.IdPlanet = planet.IdPlanet;

            StatusesPlanet sp = new StatusesPlanet();
            sp.IdPlanetNavigation = planet;
            sp.IdStatus = 1;

            _db.Planets.Add(planet);
            _db.PlanetUsersInfos.Add(pui);
            _db.StatusesPlanets.Add(sp);
            _db.PlanetInfos.Add(planetInfo);
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

    }
}
