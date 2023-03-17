using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorFilterModel
	{
		public string? keyword { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public int? Year { get; set; }
		public int? Month { get; set; }
		public IEnumerable<SelectListItem>? MonthList { get; set; }

		public AuthorFilterModel()
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
