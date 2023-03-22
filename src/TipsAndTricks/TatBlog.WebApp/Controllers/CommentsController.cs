using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Controllers
{
	public class CommentsController : Controller
	{
		[HttpPost]	
		public async Task<IActionResult> Index(
			string name,string comment)
		{
			return View();
		}
	}
}
