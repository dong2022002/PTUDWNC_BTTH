using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using MapsterMapper;
using TatBlog.Core.Entities;
using TatBlog.Services.Media;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly IValidator<PostEditModel> _validator;
		private readonly ILogger<PostsController> _logger;


		public PostsController(
			ILogger<PostsController> logger,
			IValidator<PostEditModel> validator,
			IMediaManager mediaManager,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			_logger = logger;
			_validator = validator;
			_blogRepository = blogRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
		}

		[HttpPost]
		public async Task<IActionResult> setPublished(
			int id =-1
			)
		{
			if (id>0)
			{
				await _blogRepository.SetPublishedPostAsync(id);
			}
			return RedirectToAction(nameof(Index));


		}
		


		public async Task<IActionResult> DeletePost(
			int id = -1
			)
		{
            if (id>0)
            {
              var post =  await _blogRepository.DeletePostAsync(id);
			 await	_mediaManager.DeleteFileAsync(post.ImageUrl);
				
            }
			return RedirectToAction(nameof(Index));
			

		}

		public async Task<IActionResult> Index(
			PostFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5)
		
		{
       
            _logger.LogInformation("Tạo điều kiện truy vấn");
			var postQuery = _mapper.Map<PostQuery>(model);

			_logger.LogInformation("Lấy danh sách bài viết từ CSDL");

			ViewBag.PostsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

			_logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

			await PopulatePostFilterModelAsync(model);

			return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var post = id > 0
				? await _blogRepository.GetPostByIdAsync(id, true)
				: null;

			var model = post == null
				? new PostEditModel()
				: _mapper.Map<PostEditModel>(post);

			await PopulatePostFilterModelAsync(model);

			return View(model);

		}
		[HttpPost]
		public async Task<IActionResult> Edit(
			PostEditModel model)
		{
			var validationResult = await _validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			if (!ModelState.IsValid)
			{
				await PopulatePostFilterModelAsync(model);
				return View(model);
			}

			var post = model.Id > 0
				? await _blogRepository.GetPostByIdAsync(model.Id, false)
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

			if (model.ImageFile?.Length > 0)
			{
				var newImagePath = await _mediaManager.SaveFileAsync(
					model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);

				if (!string.IsNullOrWhiteSpace(newImagePath))
				{
					await _mediaManager.DeleteFileAsync(post.ImageUrl);
					post.ImageUrl = newImagePath;
				}
			}
			await _blogRepository.AddUpdatePostAsync(
				post, model.GetSelectedTags());
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(
			int id, string slug)
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
