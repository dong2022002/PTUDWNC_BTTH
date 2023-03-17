using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly IValidator<CategoryEditModel> _validator;



		public AuthorsController(
			IValidator<CategoryEditModel> validator,
			IMediaManager mediaManager,
			IBlogRepository blogRepository,
			IMapper mapper)
		{

			_validator = validator;
			_blogRepository = blogRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
		}
		[HttpGet]
		public async Task<IActionResult> Index(
		CategoryFilterModel model,
		[FromQuery(Name = "p")] int pageNumber = 1,
		[FromRoute(Name = "filterIsShowOnMenu")] bool isShowOn = false,
		[FromQuery(Name = "isFilter")] bool isFilter = false,
		[FromQuery(Name = "ps")] int pageSize = 5)
		{


			var catsQuery = _mapper.Map<CategoryQuery>(model);

			if (isFilter)
			{
				catsQuery.isShowOnMenu = isShowOn;
			}


			ViewBag.CatsList = await _blogRepository
				.GetPagedCategoriesAsync(catsQuery, pageNumber, pageSize);

			ViewBag.filter = catsQuery;
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> setPublished(
			CategoryFilterModel model,
			int id = -1)
		{
			if (id > 0)
			{
				await _blogRepository.SetShowOnMenuCategoryAsync(id);
			}
			return RedirectToAction(nameof(Index), model);


		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var cat = id > 0
				? await _blogRepository.GetCategoryFromIDAsync(id)
				: null;

			var model = cat == null
				? new CategoryEditModel()
				: _mapper.Map<CategoryEditModel>(cat);

			return View(model);

		}
		[HttpPost]
		public async Task<IActionResult> Edit(
			CategoryEditModel model)
		{
			var validationResult = await _validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var cat = model.Id > 0
				? await _blogRepository.GetCategoryFromIDAsync(model.Id)
				: null;

			if (cat == null)
			{
				cat = _mapper.Map<Category>(model);
				cat.Id = 0;
			}
			else
			{
				_mapper.Map(model, cat);
			}
			await _blogRepository.AddUpdateCategoryAsync(cat);
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> DeleteCat(
			int id = -1)
		{
			if (id > 0)
			{
				await _blogRepository.DeleteCategoryAsync(id);

			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult DefaultFilter()
		{
			return RedirectToAction(nameof(Index));
		}

	}
}
