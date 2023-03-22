using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class CommentFilterModel
	{
		public string? Keyword { get; set; }
		public bool NotPublished { get; set; } = false;
		public int? Month { get; set; }
		public int? Year { get; set; }
		public int? PostId { get; set; }

		public IEnumerable<SelectListItem>? PostList { get; set; }
		public IEnumerable<SelectListItem>? MonthList { get; set; }

		public CommentFilterModel()
		{
			MonthList = Enumerable.Range(1, 12)
				.Select(m => new SelectListItem()
				{
					Value = m.ToString(),
					Text = CultureInfo.CurrentCulture
					.DateTimeFormat.GetMonthName(m)
				})
				.ToList();
		}
	}
}
