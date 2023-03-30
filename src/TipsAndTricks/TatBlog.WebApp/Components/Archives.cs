using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class Archives :ViewComponent
	{
		private readonly IBlogRepository _blogRepository;

		public Archives(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			int monthNumber = 12;
			var datePost =await _blogRepository.GetPostCountByMonthArchives(monthNumber);
			return View(datePost);
		}
	}
}
