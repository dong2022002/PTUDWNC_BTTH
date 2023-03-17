using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly IValidator<PostEditModel> _validator;
		private readonly ILogger<PostsController> _logger;


		public CategoriesController(
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
		public async Task<IActionResult> Index(
		CategoryFilterModel model,
		[FromQuery(Name = "p")] int pageNumber = 1,
		[FromQuery(Name = "ps")] int pageSize = 5)
		{
			_logger.LogInformation("Tạo điều kiện truy vấn");
			var catsQuery = _mapper.Map<CategoryQuery>(model);

			_logger.LogInformation("Lấy danh sách chủ đề từ CSDL");

			ViewBag.CatsList = await _blogRepository
				.GetPagedCategoriesAsync(catsQuery, pageNumber, pageSize);

			return View(model);
		}

	
	}
}
