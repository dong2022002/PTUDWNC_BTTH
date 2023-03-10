using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class RandomPosts : ViewComponent
	{
		private readonly IBlogRepository _blogRepository;

		public RandomPosts(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			int numberPost = 5;
			var posts = await _blogRepository
								.GetPostsRandomAsync(numberPost);
			return View(posts);
		}


	}
}
