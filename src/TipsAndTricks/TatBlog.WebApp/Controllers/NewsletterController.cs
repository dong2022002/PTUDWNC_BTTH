using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
	public class NewsletterController : Controller
	{
		private readonly ISubscriberRepository _subscriberRepository;

		public NewsletterController(ISubscriberRepository subscriberRepository)
		{
			_subscriberRepository = subscriberRepository;
		}
		public async Task<IActionResult> Subscribe(
			string email)
		{
			var rs= await _subscriberRepository.SubscriberAsync(email);
   //         if (rs)
   //         {
			//	using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
			//	{
			//		client.Port = 587;
			//		// Tạo xác thực bằng địa chỉ gmail và password
			//		client.Credentials = new NetworkCredential("dong200220@gmail.com", "Dong842850");
			//		client.EnableSsl = true;

			//		await MailUtils.MailUtils.SendMailGoogleSmtp("dong200220gmail.com", email, "Chủ đề", "Nội dung",
			//									  "dong200220@gmail.com", "Dong842850");
			//	}
			//}
          
			return View(rs);
		}public async Task<IActionResult> Unsubscribe(
			string email)
		{
			return View();
		}
	}
}
