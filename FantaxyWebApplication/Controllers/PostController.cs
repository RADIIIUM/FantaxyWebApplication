using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;

namespace FantaxyWebApplication.Controllers
{
    public class PostController : Controller
    {
        FantaxyContext _db;
        public List<byte[]> Images = new List<byte[]>(3);

        IHttpContextAccessor _httpContextAccessor;
        public PostController(FantaxyContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        private async Task<IList<PostModel>> GetPostAsync(int IdPlanet)
        {
            IQueryable<PostModel> posts = from p in _db.Posts
                                          join u in _db.PlanetUsersInfos on p.OwnerLogin equals u.UserLogin into up
                                          from u in up.DefaultIfEmpty()

                                          select new PostModel()
                                          {
                                              IdPost = p.IdPost,
                                              Title = p.PostsInfo.Title,
                                              Description = p.PostsInfo.PostText,
                                              AuthorLogin = p.OwnerLogin,
                                              AuthorImagePath = u.Avatar,
                                              LikeCount = _db.LikesPosts.Count(y => y.IdPost == p.IdPost),
                                              DislikeCount = _db.DisikesPosts.Count(y => y.IdPost == p.IdPost),
                                          };

            IList<PostModel> list = await posts.AsNoTracking().ToListAsync();
            return list;
        }

        public async Task<IActionResult> OpenCreatePost()
        {
            ViewBag.OpenCreatePost = false ? true : false;
            return PartialView();
        }

        //[HttpPost]
        //public async Task<IActionResult> PostUploadFile([FromForm] IFormFile Avatar)
        //{
        //    List<byte[]> Images = new List<byte[]>(3);
        //    Images = HttpContext.Session.Get<List<byte[]>>("ImageUpload");
        //    if(Images == null && Images.Count <= 0)
        //    {
        //        if (Avatar != null && Images.Count < 3)
        //        {
        //            Images.Add(ImageUpload.UploadPostImage(_httpContextAccessor, Avatar));
        //            HttpContext.Session.Set<List<byte[]>>("ImageUpload", Images);
        //        }

        //        return Json(new { images = Images });
        //    }
        //    else
        //    {
        //        if (Avatar != null && Images.Count < 3)
        //        {
        //            Images.Add(ImageUpload.UploadPostImage(_httpContextAccessor, Avatar));
        //            HttpContext.Session.Set<List<byte[]>>("ImageUpload", Images);
        //        }
        //    }

        //}

        [HttpPost]
        public IActionResult DeleteImage(int index)
        {
            if (index >= 0 && index < Images.Count)
            {
                Images.RemoveAt(index);
                return Json(new { result = "success", images = Images });
            }
            else
            {
                return Json(new { result = "error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string Title, string Desc)
        {
            int IdPlanet = HttpContext.Session.Get<int>("PlanetId");

            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            if (IdPlanet == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(Desc))
            {
                return RedirectToAction("PlanetProfile", "Planet");
            }

            Post post = new Post();
            post.OwnerLogin = userModel.Login;
            post.IdPlanet = IdPlanet;

            PostsInfo postInfo = new PostsInfo();
            postInfo.IdPostNavigation = post;
            postInfo.PostText = Desc;
            postInfo.Title = Title;

            _db.Posts.Add(post);
            _db.PostsInfos.Add(postInfo);

            return RedirectToAction("PlanetProfile", "Planet");
        }
    }
}
