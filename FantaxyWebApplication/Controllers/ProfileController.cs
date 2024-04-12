using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;

namespace FantaxyWebApplication.Controllers
{

    public class ProfileController : Controller
    {
        FantaxyContext _db;
        IWebHostEnvironment _appEnvironment;
        public ProfileController(FantaxyContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }


        public string CreateFileFromByteArray(byte[] data, string filePath)
        {
            using (var stream = new FileStream(_appEnvironment.WebRootPath + filePath, FileMode.Create))
            {
                stream.Write(data, 0, data.Length);
            }
            return filePath;
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
                if(edit.Avatar != null) userModel.Avatar = CreateFileFromByteArray(edit.Avatar, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Avatar", $"{userModel.Login}.jpg"));
                if(edit.Main != null) userModel.Main = CreateFileFromByteArray(edit.Main, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Main", $"{userModel.Login}.jpg"));
                if(edit.Profile != null)  userModel.Profile = CreateFileFromByteArray(edit.Profile, Path.Combine("\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Profile", $"{userModel.Login}.jpg"));
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
                HttpContext.Session.Remove("EditProfileSession");
                HttpContext.Response.Cookies.Delete("UserInfo");

                var json2 = JsonSerializer.Serialize<UserModel>(userModel);
                HttpContext.Response.Cookies.Append("UserInfo", json2);

                return RedirectToAction("MainProfile", "Profile");
            }
        }
        public async Task<IActionResult> EditProfile ()
        {
            var result = HttpContext.Session.Get<EditModel>("EditProfile");
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
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            HttpContext.Session.Set<byte[]>("UploadAvatar", avatar);
            var model = HttpContext.Session.Get<EditModel>("EditProfile");
            model.Avatar = avatar;
            HttpContext.Session.Remove("EditModel");
            HttpContext.Session.Set<EditModel>("EditProfile", model);

            return View("EditProfile", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMain([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            HttpContext.Session.Set<byte[]>("UploadAvatar", avatar);
            var model = HttpContext.Session.Get<EditModel>("EditProfile");
            model.Main = avatar;
            HttpContext.Session.Remove("EditModel");
            HttpContext.Session.Set<EditModel>("EditProfile", model);

            return View("EditProfile", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfile([FromForm] IFormFile Avatar)
        {
            byte[] avatar = ImageUpload.UploadImage(Avatar);
            HttpContext.Session.Set<byte[]>("UploadAvatar", avatar);
            var model = HttpContext.Session.Get<EditModel>("EditProfile");
            model.Profile = avatar;
            HttpContext.Session.Remove("EditModel");
            HttpContext.Session.Set<EditModel>("EditProfile", model);

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

            
            return Redirect("/Main/Main");
        }
    }
}
