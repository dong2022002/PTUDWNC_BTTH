using Microsoft.AspNetCore.Mvc.Rendering;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class CategoryFilterModel
	{
		public string? keyword { get; set; }
		public string? Name { get; set; }
		public string? UrlSlug { get; set; }
		public string? Description { get; set; }
		public bool isShowOnMenu { get; set; } = false;
	}
}
