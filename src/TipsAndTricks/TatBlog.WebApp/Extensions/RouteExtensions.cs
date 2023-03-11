﻿namespace TatBlog.WebApp.Extensions
{
	public static class RouteExtensions
	{

		public static IEndpointRouteBuilder UseBlogRoutes(
			this IEndpointRouteBuilder endpoint)
		{

			endpoint.MapControllerRoute(
				name: "submit-email",
				pattern: "Newsletter/Subscribe/{email}",
				defaults: new { controller = "Newsletter", action = "Subcribe" });

			endpoint.MapControllerRoute(
				name: "posts-by-category",
				pattern: "blog/category/{slug}",
				defaults: new { controller = "Blog", action = "Category" });

			endpoint.MapControllerRoute(
				name: "posts-by-author",
				pattern: "blog/author/{slug}",
				defaults: new { controller = "Blog", action = "author" });

			endpoint.MapControllerRoute(
				name: "posts-by-tag",
				pattern: "blog/Tag/{slug}",
				defaults: new { controller = "Blog", action = "Tag" });

			endpoint.MapControllerRoute(
				name: "single-post",
				pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
				defaults: new { controller = "blog", action = "post" });

			endpoint.MapControllerRoute(
				name: "admin-area",
				pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
				defaults: new { area = "Admin" });

			endpoint.MapControllerRoute(
				name: "default",
				pattern: "{controller=Blog}/{action=Index}/{id?}");

			return endpoint;
		}

	}
}
