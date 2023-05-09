using NetCoreWebGoat.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebGoat.Models;
using System.IO;
using System.Threading.Tasks;
using NetCoreWebGoat.Helpers;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace NetCoreWebGoat.Controllers
{
    [Route("[controller]")]
    public class PostController : BaseController
    {
        private readonly PostRepository _postRepository;

        private readonly CommentRepository _commentRepository;

        private readonly UserRepository _userRepository;

        public PostController(ILogger<PostController> logger,
            PostRepository postRepository,
            CommentRepository commentRepository,
            UserRepository userRepository) : base(logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string search)
        {
            ViewBag.Search = search ?? "";
            var posts = _postRepository.GetAll(search);
            var comments = _commentRepository.GetAll();
            var users = _userRepository.GetAll();

            foreach (var comment in comments)
            {
                comment.Author = users.First(p => p.Id == comment.UserId);
                comment.Owner = comment.UserId == UserId;
            }

            foreach (var post in posts)
            {
                post.Owner = post.UserId == UserId;
                post.Comments = comments.Where(p => p.PostId == post.Id).ToList();
                foreach (var comment in post.Comments)
                    comment.Owner = comment.Owner || post.Owner;
            }
            return View(posts);
        }

        [Authorize]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PostModel model)
        {
            if (model.File is null || model.File.Length == 0)
            {
                ModelState.AddModelError(nameof(model.File), "Invalid file.");
            }

            if (ModelState.IsValid)
            {
                model.Photo = HashHelper.Md5(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")).Substring(0, 6) + model.File.FileName;
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", model.Photo);
                model.UserId = UserId;
                using (var stream = new FileStream(pathToSave, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                _postRepository.Add(model);
                return Redirect("/Post");
            }
            return View();
        }

        [Authorize]
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _postRepository.Delete(id);
            return Redirect("/Post");
        }

        protected override void Dispose(bool disposing)
        {
            _postRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}