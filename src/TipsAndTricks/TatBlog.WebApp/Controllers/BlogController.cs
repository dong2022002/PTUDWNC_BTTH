using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController :Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper _mapper;

		public BlogController(
			IBlogRepository blogRepository,
            ILogger<BlogController> logger,
			ICommentRepository commentRepository, IMapper mapper,
			IAuthorRepository authorRepository

		)
        {
            _blogRepository = blogRepository;
			_logger = logger;
			_logger.LogDebug(1, "NLog injected into Blogcontroller");
			_mapper = mapper;
			_commentRepository = commentRepository;
			_authorRepository = authorRepository;
			
        }

		[HttpGet]
        public async Task<IActionResult> Index(
			string input,
            [FromQuery(Name ="k")] string keyword =null!,
            [FromQuery(Name ="p")] int pageNumber =1,
            [FromQuery(Name ="ps")] int pageSize =5)
        {
            _logger.LogInformation("Hello, this is the index!");
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                Keyword = keyword
            };


            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);
        

            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }
		
	

		public async Task<IActionResult> Category(
			string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5
			)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				CategorySlug = slug
			};
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;
            var cat = await _blogRepository
                .GetCategoryFromSlugAsync(slug);

			ViewBag.NameCat = cat.Name?? "Không tìm thấy chủ đề";
            return View(postsList);
        }
		public async Task<IActionResult> Author(
		   string slug,
		   [FromQuery(Name = "p")] int pageNumber = 1,
		   [FromQuery(Name = "ps")] int pageSize = 5
		   )
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				AuthorSlug = slug
			};
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuery = postQuery;
			var author = await _authorRepository
				.GetAuthorBySlugAsync(slug);

			ViewBag.NameAuthor = author.FullName;
			return View(postsList);
		}

		public async Task<IActionResult> Tag(
		   string slug,
		   [FromQuery(Name = "p")] int pageNumber = 1,
		   [FromQuery(Name = "ps")] int pageSize = 5
		   )
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				TagSlug = slug
			};
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuery = postQuery;
			var tag = await _blogRepository
				.GetTagFromSlugAsync(slug);

			ViewBag.NameTag = tag.Name;
			return View(postsList);
		}

		public async Task<IActionResult> Post(
				string slug,
				int year,
				int month,
				int day)
		{
            _logger.LogInformation("Hello, this is the Post!");
			
            var post = await _blogRepository
				.GetPostAsync(year, month, slug);
			await _blogRepository.IncreaseViewCountAsync(post.Id);
			var conditionComment = new CommentQuery()
			{
				PostId = post.Id,
				PublishedOnly = true,
			};
			var comments = await _commentRepository.GetPagedCommentsAsync(conditionComment);
			ViewBag.Comments = comments;
			return View(post);
		}


		public async Task<IActionResult> Archives(
				int year,
				int month)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				YearPost = year,
				MonthPost = month
			};
			var postList =await _blogRepository.GetPagedPostsAsync(postQuery);

			ViewBag.PostQuery = postQuery;

			return View(postList);
		}
		[HttpPost]
		public async Task<IActionResult> Comments(
			int id,
			string name, string comment)
		{
			var newComment = new CommentItem()
			{
				Name = name,
				Description = comment,
				Published = false,
				DateComment = DateTime.Now,
				PostId = id
			};
			Comment data = _mapper.Map<Comment>(newComment);
			await _commentRepository.AddUpdateCommentAsync(data);
			return RedirectToAction(nameof(Post));
		}
		public IActionResult About() 
		{ 
			return View();                    
		}
        public IActionResult Contact()
		{
			return View();	
		}
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

    }
}
