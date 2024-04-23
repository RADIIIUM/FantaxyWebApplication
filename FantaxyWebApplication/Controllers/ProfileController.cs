using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;

namespace FantaxyWebApplication.Controllers
{

    public class ProfileController : Controller
    {
        FantaxyContext _db;
        IWebHostEnvironment _appEnvironment;
        IHttpContextAccessor _contextAccessor;
        public ProfileController(FantaxyContext db, IWebHostEnvironment appEnvironment, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _appEnvironment = appEnvironment;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Save(string Nickname, string Desc)
        {
            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            userModel.Name = Nickname ?? userModel.Name;
            userModel.Description = Desc ?? userModel.Description;

            EditModel edit = HttpContext.Session.Get<EditModel>("EditProfile");
            if (edit != null)
            {
                if(edit.Avatar != null) userModel.Avatar = FileServices.CreateFileFromByteArray(_appEnvironment, edit.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Avatar", $"{userModel.Login}.jpg"));
                if(edit.Main != null) userModel.Main = FileServices.CreateFileFromByteArray(_appEnvironment, edit.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Main", $"{userModel.Login}.jpg"));
                if(edit.Profile != null)  userModel.Profile = FileServices.CreateFileFromByteArray(_appEnvironment, edit.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Profile", $"{userModel.Login}.jpg"));
            }

            using (FantaxyContext db = new FantaxyContext())
            {
                GlobalUsersInfo uf = db.GlobalUsersInfos.FirstOrDefault(x => x.UserLogin == userModel.Login);
                uf.Avatar = userModel.Avatar;
                uf.UserName = userModel.Name;
                uf.UserDescription = userModel.Description;
                uf.MainBackground = userModel.Main;
                uf.ProfileBackground = userModel.Profile;
                db.GlobalUsersInfos.Update(uf);
                await db.SaveChangesAsync();


                HttpContext.Session.Remove("EditProfile");
                HttpContext.Response.Cookies.Delete($"UserInfo");

                var json2 = JsonSerializer.Serialize<UserModel>(userModel);
                HttpContext.Response.Cookies.Append($"UserInfo", json2);
                return RedirectToAction("MainProfile", "Profile");
            }
        }

        public async Task<IActionResult> SavePlanetProfile(string Nickname, string Desc)
        {
            int IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            userModel.Name = Nickname ?? userModel.Name;
            userModel.Description = Desc ?? userModel.Description ?? "Hello world!";

            EditModel edit = HttpContext.Session.Get<EditModel>($"EditProfile");
            if (edit != null)
            {
                if (edit.Avatar != null) userModel.Avatar = FileServices.CreateFileFromByteArray(_appEnvironment, edit.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Profiles\\Avatar", $"{userModel}_{IdPlanet}.jpg"));
                if (edit.Main != null) userModel.Main = FileServices.CreateFileFromByteArray(_appEnvironment, edit.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Profiles\\Main", $"{userModel.Login}_{IdPlanet}.jpg"));
                if (edit.Profile != null) userModel.Profile = FileServices.CreateFileFromByteArray(_appEnvironment, edit.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Profiles\\Profile", $"{userModel.Login}_{IdPlanet}.jpg"));
            }

            using (FantaxyContext db = new FantaxyContext())
            {
                PlanetUsersInfo uf = db.PlanetUsersInfos.FirstOrDefault(x => x.IdPlanet == IdPlanet && x.UserLogin == userModel.Login);
                uf.Avatar = userModel.Avatar;
                uf.UserName = userModel.Name;
                uf.UserDescription = userModel.Description;
                uf.MainBackground = userModel.Main;
                uf.ProfileBackground = userModel.Profile;
                db.PlanetUsersInfos.Update(uf);
                await db.SaveChangesAsync();

                HttpContext.Session.Remove("EditProfile");
                HttpContext.Response.Cookies.Delete($"Profile_{IdPlanet}");

                var json2 = JsonSerializer.Serialize<UserModel>(userModel);
                HttpContext.Response.Cookies.Append($"Profile_{IdPlanet}", json2);
            }

            return RedirectToAction("PlanetProfile", "Profile");
        }

        public async Task<IActionResult> EditProfile ()
        {
            EditModel? result = HttpContext.Session.Get<EditModel>("EditProfile");
            if (result != null)
            {
                return View(result);
            }

            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            if (userModel != null)
            {
                EditModel edit = new EditModel();
                edit.Login = userModel.Login;
                edit.Name = userModel.Name;
                edit.Email = userModel.Email;
                edit.Role = userModel.Role;
                edit.Telephone = userModel.Telephone;
                edit.Description = userModel.Description;
                edit.Avatar = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, userModel.Avatar));
                edit.Main = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, userModel.Main));
                edit.Profile = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, userModel.Profile));

                HttpContext.Session.Set<EditModel>("EditProfile", edit);
                return View(edit);
            }
            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> UploadAvatarPlanet([FromForm] IFormFile Avatar, string tag)
        {
            EditModel? model = HttpContext.Session.Get<EditModel>("EditProfile");
            if (tag == "_A") model.Avatar = ImageUpload.UploadProfileImage(_contextAccessor, Avatar, "EditProfile", model, tag);
            if(tag == "_M") model.Main = ImageUpload.UploadProfileImage(_contextAccessor, Avatar, "EditProfile", model, tag);
            if (tag == "_P") model.Profile = ImageUpload.UploadProfileImage(_contextAccessor, Avatar, "EditProfile", model, tag);
            return View("EditProfilePlanet", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile Avatar, string tag)
        {
            EditModel? model = HttpContext.Session.Get<EditModel>("EditProfile");
            if (tag == "_A") model.Avatar = ImageUpload.UploadProfileImage(_contextAccessor, Avatar, "EditProfile", model, tag);
            if (tag == "_M") model.Main = ImageUpload.UploadProfileImage(_contextAccessor, Avatar, "EditProfile", model, tag);
            if (tag == "_P") model.Profile = ImageUpload.UploadProfileImage(_contextAccessor, Avatar, "EditProfile", model, tag);
            return View("EditProfile", model);
        }

        public async Task<IActionResult> MainProfile()
        {
            HttpContext.Session.Remove("EditProfile");
            var json = HttpContext.Request.Cookies["UserInfo"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

                if(userModel != null)
                {
                    return View(userModel);
                }

            
            return Redirect("/Main/Users");
        }

        public async Task<IActionResult> PlanetProfile(int IdPlanet)
        {
            IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            HttpContext.Session.Remove("EditProfile");
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];

            if (json != null)
            {
                UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
                return View(userModel);
            }
            else
            {
                return Redirect("/Planet/MainPage");

            }

        }



        public async Task<IActionResult> EditProfilePlanet(int IdPlanet)
        {
            IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            EditModel? edit = HttpContext.Session.Get<EditModel>("EditProfile");

            if (edit != null)
            {
                return View(edit);
            }
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            if (userModel != null)
            {
                edit = new EditModel();
                edit.Login = userModel.Login;
                edit.Name = userModel.Name;
                edit.Email = userModel.Email;
                edit.Role = userModel.Role;
                edit.Telephone = userModel.Telephone;
                edit.Description = userModel.Description;
                edit.Avatar = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, userModel.Avatar));
                edit.Main = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, userModel.Main));
                edit.Profile = ImageUpload.ConvertIFormFileToByteArray(FileServices.ConvertFileToIFormFile(_appEnvironment, userModel.Profile));

                HttpContext.Session.Set<EditModel>("EditProfile", edit);
                return View(edit);
            }

            return Redirect("/Main/Users");
        }

        [HttpGet]
        public async Task<string> GetRole(PlanetUsersInfo userModel, int IdPlanet)
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
