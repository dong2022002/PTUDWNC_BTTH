using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using MapsterMapper;
using TatBlog.Core.Entities;

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
		[HttpGet]
		public async Task<IActionResult> Edit(int id =0)
		{
            
            var post = id >0
				? await _blogRepository.GetPostByIdAsync(id,true)
				: null;

			var model = post == null
				? new PostEditModel() 
				: _mapper.Map<PostEditModel>(post);

			await PopulatePostFilterModelAsync(model);

			return View(model);

		}
		[HttpPost]
		public async Task<IActionResult> Edit(PostEditModel model)
		{
			if (!ModelState.IsValid)
			{
				await PopulatePostFilterModelAsync(model);
				return View(model);
			}

			var post = model.Id > 0
				? await _blogRepository.GetPostByIdAsync(model.Id,false)
			
				: null;
			
			if (post == null)
			{
				post = _mapper.Map<Post>(model);

				post.Id = 0;
				post.PostedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, post);
				post.Category = null;
				post.ModifiedDate = DateTime.Now;
			}
			await _blogRepository.AddUpdatePostAsync(
				post, model.GetSelectedTags());
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(
			int id,string slug)
		{ 
			var slugExisted = await _blogRepository
				.IsPostSlugExitedAsync(id, slug);

			return slugExisted
				? Json($"Slug '{slug}' đã được sử dụng")
				: Json(true);
		}

            private async Task PopulatePostFilterModelAsync<T>(T model)
		{
          
            var authors = await _blogRepository.GetAuthorsAsync();
			var categories = await _blogRepository.GetCategoriesAsync();
			if (model is PostFilterModel)
			{
				(model as PostFilterModel)!.AuthorList = authors.Select(a => new SelectListItem()
				{
					Text = a.FullName,
					Value = a.Id.ToString()
				});

				(model as PostFilterModel)!.CategoryList = categories.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
			}
            else if (model is PostEditModel)
            {
				(model as PostEditModel)!.AuthorList = authors.Select(a => new SelectListItem()
				{
					Text = a.FullName,
					Value = a.Id.ToString()
				});

				(model as PostEditModel)!.CategoryList = categories.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
			}

        }
		
	}
}
