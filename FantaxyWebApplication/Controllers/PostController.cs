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
            var posts = _db.Posts
               .Where(p => p.IdPlanet == IdPlanet)
               .Select(p => new PostModel
               {
                   IdPost = p.IdPost,
                   Title = p.PostsInfo.Title,
                   Description = p.PostsInfo.PostText,
                   authorInfo = p.OwnerLoginNavigation.PlanetUsersInfos.FirstOrDefault(a => a.IdPlanet == p.IdPlanet),
                   Files = p.PostFiles.Select(pf => pf.PathFile).ToList(),
                   LikeCount = p.LikeDislikePosts.Count(ldp => ldp.LikeOrDislike),
                   DislikeCount = p.LikeDislikePosts.Count(ldp => !ldp.LikeOrDislike)
               });

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

                _db.PostFiles.Add(postFile);
            }
            _db.PostsInfos.Add(postInfo);

            await _db.SaveChangesAsync();
            return RedirectToAction("PlanetProfile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> GetLikeDislikeStatus(int postId)
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);
            var likeStatus = await _db.LikeDislikePosts.AnyAsync(l => l.IdPost == postId && l.UserLogin == userModel.Login && l.LikeOrDislike == true);
            var dislikeStatus = await _db.LikeDislikePosts.AnyAsync(d => d.IdPost == postId && d.UserLogin == userModel.Login && d.LikeOrDislike == false);

            return Ok(new { likeStatus, dislikeStatus });
        }


        [HttpPost]
        public async Task<IActionResult> LikeOrDislike(int postId, string type)
        {
            int? IdPlanet = HttpContext.Session.Get<int>("PlanetId");
            var json = HttpContext.Request.Cookies[$"Profile_{IdPlanet}"];
            UserModel? userModel = JsonSerializer.Deserialize<UserModel>(json);

            if (userModel != null)
            {
                if (type == "like")
                {
                    LikeDislikePost? like = _db.LikeDislikePosts.FirstOrDefault(x => x.IdPost == postId &&
                    x.UserLogin == userModel.Login);

                    if(like != null && like.LikeOrDislike == true)
                    {
                        _db.LikeDislikePosts.Remove(like);
                        await _db.SaveChangesAsync();

                        return Ok(new
                        {
                            likeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == true),
                            dislikeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == false)
                        });
                    }

                    like = new LikeDislikePost();
                    like.IdPost = postId;
                    like.UserLogin = userModel.Login;
                    like.LikeOrDislike = true;
                    LikeDislikePost? likeOrDislike = _db.LikeDislikePosts.FirstOrDefault(x => x.IdPost == postId && x.UserLogin == userModel.Login);
                    if (likeOrDislike != null)
                    {
                        _db.LikeDislikePosts.Remove(likeOrDislike);
                    }

                    _db.LikeDislikePosts.Add(like);
                    await _db.SaveChangesAsync();

                    return Ok(new
                    {
                        likeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == true),
                        dislikeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == false)
                    });
                }
                else
                {
                    LikeDislikePost? dislike = _db.LikeDislikePosts.FirstOrDefault(x => x.IdPost == postId &&
                    x.UserLogin == userModel.Login);

                    if (dislike != null && dislike.LikeOrDislike == false)
                    {
                        _db.LikeDislikePosts.Remove(dislike);
                        await _db.SaveChangesAsync();

                        return Ok(new
                        {
                            likeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == true),
                            dislikeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == false)
                        });
                    }

                    dislike = new LikeDislikePost();
                    dislike.IdPost = postId;
                    dislike.UserLogin = userModel.Login;
                    dislike.LikeOrDislike = false;
                    LikeDislikePost? likeOrDislike = _db.LikeDislikePosts.FirstOrDefault(x => x.IdPost == postId && x.UserLogin == userModel.Login);
                    if (likeOrDislike != null)
                    {
                        _db.LikeDislikePosts.Remove(likeOrDislike);
                    }
                    _db.LikeDislikePosts.Add(dislike);
                    await _db.SaveChangesAsync();

                    return Ok(new
                    {
                        dislikeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == false),
                        likeCount = _db.LikeDislikePosts.Count(l => l.IdPost == postId && l.LikeOrDislike == true)
                    });
                }
            }
            return NotFound();
            
        } 
    }
}
