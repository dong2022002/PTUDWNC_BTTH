using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class SubscriberController : Controller
	{
		private readonly ISubscriberRepository _subscriberRepository;
		private readonly IMapper _mapper;
		private readonly IValidator<SubscriberEditModel> _validator;

        public SubscriberController(
			IValidator<SubscriberEditModel> validator,
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
		[HttpGet]
		public async Task<IActionResult> Block(int id = 0)
		{
			var subscriber = id > 0
				? await _subscriberRepository.GetSubscriberByIdAsync(id)
				: null;

			var model = subscriber == null
				? new SubscriberEditModel()
				: _mapper.Map<SubscriberEditModel>(subscriber);
			ViewBag.Name = subscriber!.Mail;
			return View(model);

		}
		[HttpPost]
		public async Task<IActionResult> Block(
			SubscriberEditModel model)
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

			await _subscriberRepository.BlockSubscriberAsync(model.Id, model.NoteAdmin, model.Desc);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> DeleteSubscriber(
			int id = -1)
		{
			if (id > 0)
			{
				await _subscriberRepository.DeleteSubscriberAsync(id);

			}
			return RedirectToAction(nameof(Index));
		}

	}
}
