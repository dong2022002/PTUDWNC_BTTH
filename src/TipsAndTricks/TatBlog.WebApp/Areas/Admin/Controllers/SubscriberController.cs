﻿using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class SubscriberController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
