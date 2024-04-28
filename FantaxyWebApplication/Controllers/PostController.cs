using FantaxyWebApplication.Models;
using FantaxyWebApplication.Models.Entities;
using FantaxyWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Linq;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace FantaxyWebApplication.Controllers
{
    public class PostController : Controller
    {
        FantaxyContext _db;

        IHttpContextAccessor _httpContextAccessor;
        IWebHostEnvironment _webHostEnvironment;
        public PostController(FantaxyContext db, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        private async Task<IList<PostModel>> GetPostAsync(int IdPlanet)
        {
            IQueryable<PostModel> posts = from p in _db.Posts
                                          join u in _db.PlanetUsersInfos on p.OwnerLogin equals u.UserLogin into up
                                          from u in up.DefaultIfEmpty()
                                          join files in _db.PostFiles on p.IdPost equals files.IdPost into filesPost
                                          from files in filesPost.DefaultIfEmpty()
                                          where p.IdPlanet == IdPlanet
                                          select new PostModel()
                                          {
                                              IdPost = p.IdPost,
                                              Title = p.PostsInfo.Title,
                                              Description = p.PostsInfo.PostText,
                                              Files = filesPost.Select(x => x.PathFile).ToList<string>(),
                                              authorInfo = u,
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

        [HttpPost]
        public async Task<IActionResult> PostUploadFile([FromForm] IFormFile Avatar)
        {
            List<byte[]> Images = HttpContext.Session.Get<List<byte[]>>("PostImages") ?? new List<byte[]>();
            if (Avatar != null && Images?.Count < 3)
            {
                Images.Add(ImageUpload.UploadPostImage(_httpContextAccessor, Avatar));
                HttpContext.Session.Set("PostImages", Images);
            }
            return Json(new { images = Images });

        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string Title, string Desc)
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            List<byte[]> Images = HttpContext.Session.Get<List<byte[]>>("PostImages") ?? new List<byte[]>();
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            if (IdPlanet == null || userModel == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(Desc))
            {
                return RedirectToAction("PlanetProfile", "Profile");
            }

            Post post = new Post();
            post.OwnerLogin = userModel.Login;
            post.IdPlanet = IdPlanet;

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            PostsInfo postInfo = new PostsInfo();
            postInfo.IdPostNavigation = post;
            postInfo.PostText = Desc;
            postInfo.Title = Title;

            PostFile postFile = new PostFile();
            if (Images != null && Images.Count > 0)
            {
                postFile.IdPostNavigation = post;
                foreach(var images in Images)
                {
                    postFile.PathFile = FileServices.CreateFileFromByteArray(_webHostEnvironment, images, Path.Combine("\\img\\FantasyFiles\\Images\\Post\\", $"Post_{userModel.Login}_{post.IdPost}.jpg"));
                }
            }
            _db.PostsInfos.Add(postInfo);
            _db.PostFiles.Add(postFile);

            await _db.SaveChangesAsync();
            return RedirectToAction("PlanetProfile", "Profile");
        }


        //[HttpPost("{postId}")]
        //public async Task<ActionResult<DisikesPost> PostLikeDislike(int postId, LikeDislike likeDislike)
        //{
        //    likeDislike.PostId = postId;
        //    likeDislike.CreatedAt = DateTime.UtcNow;

        //    _context.LikeDislikes.Add(likeDislike);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}



    }
}
