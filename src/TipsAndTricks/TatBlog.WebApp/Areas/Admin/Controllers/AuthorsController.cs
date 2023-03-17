using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly IAuthorRepository _authorRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly IValidator<AuthorEditModel> _validator;



		public AuthorsController(
			IValidator<AuthorEditModel> validator,
			IMediaManager mediaManager,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{

			_validator = validator;
			_authorRepository = authorRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
		}
		[HttpGet]
		public async Task<IActionResult> Index(
		AuthorFilterModel model,
		[FromQuery(Name = "p")] int pageNumber = 1,
		[FromQuery(Name = "ps")] int pageSize = 5)
		{
			var authorQuery = _mapper.Map<AuthorQuery>(model);

			ViewBag.AuthorsList = await _authorRepository
				.GetPagedAuthorsAsync(authorQuery, pageNumber, pageSize);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var author = id > 0
				? await _authorRepository.GetAuthorFromIDAsync(id)
				: null;

			var model = author == null
				? new AuthorEditModel()
				: _mapper.Map<AuthorEditModel>(author);

			return View(model);

		}
		[HttpPost]
		public async Task<IActionResult> Edit(
			AuthorEditModel model)
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

			var author = model.Id > 0
				? await _authorRepository.GetAuthorFromIDAsync(model.Id)
				: null;

			if (author == null)
			{
				author = _mapper.Map<Author>(model);
				author.Id = 0;
				author.JoinedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, author);
			}
			if (model.ImageFile?.Length > 0)
			{
				var newImagePath = await _mediaManager.SaveFileAsync(
					model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);

				if (!string.IsNullOrWhiteSpace(newImagePath))
				{
					await _mediaManager.DeleteFileAsync(author.ImageUrl);
					author.ImageUrl = newImagePath;
				}
			}
			await _authorRepository.AddUpdateAuthorAsync(author);
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> DeleteCat(
			int id = -1)
		{
			if (id > 0)
			{
			 var author =	await _authorRepository.DeleteAuthorAsync(id);

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
