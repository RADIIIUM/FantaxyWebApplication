﻿using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace FantaxyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private FantaxyContext? _db;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, FantaxyContext db, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Request.Cookies.ContainsKey("UserInfo"))
                {
                    return Redirect("/Main/Main");
                }
                else
                {
                    GlobalUsersInfo glu = await _db.GlobalUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == User.Identity.Name);
                    UserModel us = new UserModel();
                    us.Name = glu.UserName;
                    us.Avatar = glu.Avatar;
                    us.Main = glu.MainBackground;
                    us.Profile = glu.ProfileBackground;
                    us.Description = glu.UserDescription;
                    us.Login = glu.UserLogin;
                    us.Email = glu.UserEmail;
                    us.Telephone = glu.UserTelephone;
                    us.Role = await GetRole(glu);
                    // аутентификация
                    var json = JsonSerializer.Serialize<UserModel>(us);
                    HttpContext.Response.Cookies.Append("UserInfo", json);
                    return Redirect("/Main/Main");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegAccount([FromForm]User user)
        {
            if(string.IsNullOrEmpty(user.UserLogin))
            {
                ModelState.AddModelError("UserLogin", "Пустой логин");
                return View("Registration");
            }

            if (string.IsNullOrEmpty(user.UserPassword))
            {
                ModelState.AddModelError("UserPassword", "Пустой пароль");
                return View("Registration");
            }

            if(user.UserPassword.Length > 50 || user.UserPassword.Length < 6)
            {
                ModelState.AddModelError("UserPassword", "Меньше 6 символов");

            }
          
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("UserLogin", "Ошибка");
                ModelState.AddModelError("UserPassword", "Ошибка");
                return View("Registration");
            }
            using (FantaxyContext db = new FantaxyContext())
            {
                User us = db.Users.FirstOrDefault(x => x.UserLogin == user.UserLogin);
                if (us != null)
                {
                    ModelState.AddModelError("UserLogin", "Данный пользователь уже существует");
                    return View("Registration");
                }
                User u = new User();
                u.UserLogin = user.UserLogin;
                u.UserPassword = user.UserPassword;

                GlobalUsersInfo userInfo = new GlobalUsersInfo();
                userInfo.UserLoginNavigation = u;
                userInfo.UserName = user.UserLogin;

                IFormFile? result = await UploadFile(user.UserLogin);
                if (result != null)
                {

                    string uploadsPath = "\\img\\FantasyFiles\\Profiles\\Style\\Globals\\Avatar";
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }
                    string filePath = Path.Combine(uploadsPath, result.FileName);
                    using (var filestream = new FileStream(_appEnvironment.WebRootPath + filePath, FileMode.Create))
                    {
                        await result.CopyToAsync(filestream);
                    }
                    userInfo.Avatar = filePath;

                }
                else
                {
                    userInfo.Avatar = "\\img\\icon\\stdAvatar.png";

                }
                userInfo.MainBackground = "\\img\\background\\MainBackground.jpg";
                userInfo.ProfileBackground = "\\img\\background\\secondBack.jpg";

                GlobalRoleUser glr = new GlobalRoleUser();
                glr.IdRole = 4; // 4 - Id номер Пользователя , 3 - Модератор, 2 - Админ, 1 - Главный админ, 5 - Заблокированный
                glr.UserLoginNavigation = u;
                await Task.Run(async () =>
                {
                    db.Users.Add(u);
                    db.GlobalRoleUsers.Add(glr);
                    db.GlobalUsersInfos.Add(userInfo);
                    db.SaveChanges();

                });

                ViewBag.Success = $"Пользователь {user.UserLogin} зарегестрирован!";
                return View("Registration");

            }
            return View(user);
        }

        public async Task<IActionResult> UploadImage([FromForm]IFormFile Avatar)
        {
            HttpContext.Session.Set<byte[]>("RegAvatar", ImageUpload.UploadImage(Avatar));
            return View("Registration");
        }

        public async Task<IFormFile>? UploadFile(string login)
        {
            byte[]? byteArray = HttpContext.Session.Get<byte[]>("RegAvatar");
            var ms = new MemoryStream();
            try 
            {
                if (byteArray != null)
                {
                    await ms.WriteAsync(byteArray, 0, byteArray.Length);
                    IFormFile result = new FormFile(ms, 0, byteArray.Length, $"{login}", $"{login}.jpg");
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            { 
                ms.Close(); 
            }


        }

        [HttpGet]
        public async Task<string> GetRole(GlobalUsersInfo userModel)
        {

            if (userModel == null)
            {
                return "";
            }

            var roleId = _db.GlobalRoleUsers.FirstOrDefault(x => x.UserLogin == userModel.UserLogin)?.IdRole;
            if (roleId == null)
            {
                return "";
            }

            var roleName = _db.GlobalRoles.FirstOrDefault(x => x.IdRole == roleId)?.RoleName;
            if (roleName == null)
            {
                return "";
            }

            return roleName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm]User user)
        {
            if (ModelState.IsValid)
            {
                User? u = await _db.Users.FirstOrDefaultAsync(x => x.UserPassword == user.UserPassword && x.UserLogin == user.UserLogin);
                if (u != null)
                {
                    GlobalUsersInfo glu = await _db.GlobalUsersInfos.FirstOrDefaultAsync(x => x.UserLogin == user.UserLogin);
                    UserModel us = new UserModel();
                    us.Name = glu.UserName;
                    us.Avatar = glu.Avatar;
                    us.Main = glu.MainBackground;
                    us.Profile = glu.ProfileBackground;
                    us.Description = glu.UserDescription;
                    us.Login = glu.UserLogin;
                    us.Email = glu.UserEmail;
                    us.Telephone = glu.UserTelephone;
                    us.Role = await GetRole(glu);
                    // аутентификация
                   
                    var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve };
                    var json = JsonSerializer.Serialize<UserModel>(us, options);
  
                    HttpContext.Response.Cookies.Append("UserInfo", json);
                    await Authenticate(user.UserLogin);
                    return Redirect("/Main/Main");

                }
                else
                {
                    ModelState.AddModelError("UserLogin", "Некорректные логин и(или) пароль");
                    ModelState.AddModelError("UserPassword", "Некорректные логин и(или) пароль");
                }

            }
            return View(user);

        }
        private async Task Authenticate(string user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user),
            };
            
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("UserInfo");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}