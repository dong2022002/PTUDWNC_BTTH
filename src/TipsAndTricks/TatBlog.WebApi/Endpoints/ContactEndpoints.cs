using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class ContactEndpoints
	{
		public static WebApplication MapContactEndpoints(
		this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/contact");

			routeGroupBuilder.MapGet("/", Contact)
				.WithName("Contact");

			return app;
		}

		private static IResult Contact(
			string email,
			 string title,
			 string content)
		{
			content = content.Replace("\n", "<br>");
			var subject = $"Phản hồi từ {email}";
			var body = $"{title}:<br> {content}";

			SendMailExtensions.SendEmail("dong200220@gmail.com", subject, body);
			return Results.Ok(ApiResponse.Success("Gửi thành công"));
		}
	}
}
