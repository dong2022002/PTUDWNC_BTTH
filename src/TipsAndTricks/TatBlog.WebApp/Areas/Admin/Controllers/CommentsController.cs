using FluentValidation;
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
	
	public class CommentsController : Controller
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
        public CommentsController(
			IBlogRepository blogRepository,
			IMediaManager mediaManager,
			ICommentRepository commentRepository,
			IMapper mapper)
        {
			_blogRepository = blogRepository;
            _commentRepository = commentRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
        }
        public async Task<IActionResult> IndexAsync(
			CommentFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5)
		{
			var commentQuery = _mapper.Map<CommentQuery>(model);

			ViewBag.CommentList = await _commentRepository
				.GetPagedCommentsAsync(commentQuery, pageNumber, pageSize);

			await PopulateCommentFilterModelAsync(model);

			return View(model);
		}

		private async Task PopulateCommentFilterModelAsync(CommentFilterModel model)
		{
			var posts = await _blogRepository.GetPostsFromPostQuery(new PostQuery()
			{
				PublishedOnly = true,
			});
			model.PostList = posts.Select(p => new SelectListItem()
			{
				Text = p.Title,
				Value = p.Id.ToString()
			});
		}
		public async Task<IActionResult> DeleteComment(
			int id = -1)
		{
			if (id > 0)
			{
				await _commentRepository.DeleteCommentAsync(id);
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult DefaultFilter(
			PostFilterModel model)
		{
			model = new PostFilterModel();
			return RedirectToAction(nameof(Index));

		}
	}
}
