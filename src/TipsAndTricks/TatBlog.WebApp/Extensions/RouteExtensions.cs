using Microsoft.AspNetCore.Mvc;
using System;

namespace TatBlog.WebApp.Extensions
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

			//endpoint.MapAreaControllerRoute(
			//		   name: "Admin",
			//		   areaName: "Admin",
			//		   pattern: "Admin/{controller=Dasboard}/{action=Index}/{id?}" );

			//endpoint.MapControllerRoute(
			//   name: "admin-area",
			//   pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
			//endpoint.MapControllerRoute(
			//	name: "default",
			//	pattern: "{controller=Blog}/{action=Index}/{id?}");
			endpoint.MapAreaControllerRoute(
							   name: "admin",
							   areaName: "admin",
							   pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

			endpoint.MapControllerRoute(
							name: "admin-area",
							pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

			endpoint.MapControllerRoute(
				name: "default",
				pattern: "{controller=Blog}/{action=Index}/{id?}");





			return endpoint;

		}

	}
}
