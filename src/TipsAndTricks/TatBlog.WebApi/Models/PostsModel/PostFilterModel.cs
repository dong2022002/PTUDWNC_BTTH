using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApi.Models.PostsModel
{
	public class PostFilterModel :PagingModel
	{
		public string Keyword { get; set; } =null;
		public int? AuthorId { get; set; } 
		public int? CategoryId { get; set; }
		public int? YearPost { get; set; }
		public int? MonthPost { get; set; }
		public bool PublishedOnly { get; set; } = false;
	
	}
}
