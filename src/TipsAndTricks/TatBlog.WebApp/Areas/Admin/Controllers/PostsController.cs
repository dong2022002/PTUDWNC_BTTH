using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using MapsterMapper;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;


		public PostsController(
			IBlogRepository blogRepository,
			IMapper mapper)
        {
			_blogRepository = blogRepository;
			_mapper = mapper;
        }
  //      public async Task<IActionResult> Index(
		//	PostFilterModel model)
		//{
		//	var postQuery = new PostQuery()
		//	{
		//		Keyword = model.keyword,
		//		CategoryId = model.CategoryId,
		//		AuthorId = model.AuthorId,
		//		YearPost = model.Year,
		//		MonthPost = model.Month

		//	};

		//	ViewBag.PostsList = await _blogRepository
		//		.GetPagedPostsAsync(postQuery,1,10);

		//	await PopulatePostFilterModelAsync(model);

		//	return View(model);
		//}
		public async Task<IActionResult> Index(PostFilterModel model)
		{
			var postQuery = _mapper.Map<PostQuery>(model);

			ViewBag.PostsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, 1, 10);
			await PopulatePostFilterModelAsync(model);

			return View(model);
		}


		private async Task PopulatePostFilterModelAsync(PostFilterModel model)
		{
			var authors = await _blogRepository.GetAuthorsAsync();
			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});

            model.CategoryList = categories.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });
        }
	}
}
