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
			[FromRoute(Name = "slug")] string catslug = null,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5
			)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				CategorySlug = catslug
			};
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;
            var cat = await _blogRepository
                .GetCategoryFromSlugAsync(catslug);

			ViewBag.NameCat = cat.Name?? "Không tìm thấy chủ đề";
            return View(postsList);
        }
		public async Task<IActionResult> Author(
		   [FromRoute(Name = "slug")] string authorSlug = null,
		   [FromQuery(Name = "p")] int pageNumber = 1,
		   [FromQuery(Name = "ps")] int pageSize = 5
		   )
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				AuthorSlug = authorSlug
			};
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuery = postQuery;
			var author = await _blogRepository
				.GetAuthorFromSlugAsync(authorSlug);

			ViewBag.NameAuthor = author.FullName;
			return View(postsList);
		}

		public async Task<IActionResult> Tag(
		   [FromRoute(Name = "slug")] string tagSlug = null,
		   [FromQuery(Name = "p")] int pageNumber = 1,
		   [FromQuery(Name = "ps")] int pageSize = 5
		   )
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				TagSlug = tagSlug
			};
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuery = postQuery;
			var tag = await _blogRepository
				.GetTagFromSlugAsync(tagSlug);

			ViewBag.NameTag = tag.Name;
			return View(postsList);
		}


		public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

    }
}
