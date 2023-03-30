using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class PostFilterModel
    {
        public string? keyword { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool NotPublished { get; set; } =false;
		public IEnumerable<SelectListItem>? AuthorList { get; set; }

		public IEnumerable<SelectListItem>? CategoryList { get; set; }
		public IEnumerable<SelectListItem> MonthList { get; set; }

		public PostFilterModel()
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
