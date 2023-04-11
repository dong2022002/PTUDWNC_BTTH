using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApi.Models.PostsModel
{
	public class PostDataFilter
	{
		public IEnumerable<SelectListItem> AuthorList { get; set; }

		public IEnumerable<SelectListItem> CategoryList { get; set; }
		public IEnumerable<SelectListItem> MonthList { get; set; }
        public PostDataFilter()
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
