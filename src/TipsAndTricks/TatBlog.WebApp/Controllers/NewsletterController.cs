﻿using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
	public class NewsletterController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		public NewsletterController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}
		public async Task<IActionResult> Subscribe(
			string email)
		{
			return View();
		}public async Task<IActionResult> Unsubscribe(
			string email)
		{
			return View();
		}
	}
}
