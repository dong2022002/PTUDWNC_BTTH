using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController :Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        public async Task<IActionResult> Index(
            [FromQuery(Name ="k")] string keyword =null,
            [FromQuery(Name ="p")] int pageNumber =1,
            [FromQuery(Name ="ps")] int pageSize =5)
        {
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
			var author = await _blogRepository
				.GetAuthorFromSlugAsync(slug);

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
			
			var post = await _blogRepository
				.GetPostAsync(year, month, slug);
			await _blogRepository.IncreaseViewCountAsync(post.Id);

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
			var postList = _blogRepository.GetPagedPostsAsync(postQuery);

			ViewBag.PostQuery = postQuery;

			return View(postList);
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
