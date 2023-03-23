using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class SubscriberController : Controller
	{
		private readonly ISubscriberRepository _subscriberRepository;
		private readonly IMapper _mapper;
		private readonly IValidator<PostEditModel> _validator;

        public SubscriberController(
			IValidator<PostEditModel> validator,
			ISubscriberRepository subscriberRepository,
			IMapper mapper
			)
        {
            _subscriberRepository = subscriberRepository;
			_mapper = mapper;	
			_validator = validator;
        }
		public async Task<IActionResult> Index(
			   SubcriberFilterModel model,
			   [FromQuery(Name = "p")] int pageNumber = 1,
			   [FromQuery(Name = "ps")] int pageSize = 5)
		{

			var subcriber = _mapper.Map<SubcriberQuery>(model);

			ViewBag.SubscriberList = await _subscriberRepository
				.GetPagedSubcriberAsync(subcriber, pageNumber, pageSize);


			return View(model);
		}

		
	}
}
