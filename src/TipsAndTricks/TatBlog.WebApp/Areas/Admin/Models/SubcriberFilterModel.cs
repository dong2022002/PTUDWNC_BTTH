using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class SubcriberFilterModel
	{
		public string? Keyword { get; set; }
		public int? YearRegis { get; set; }
		public int Status { get; set; } = 0;
		public int? MonthRegis { get; set; }
		public IEnumerable<SelectListItem>? StatusList { get; set; }
		public IEnumerable<SelectListItem> MonthList { get; set; }

		public SubcriberFilterModel()
		{
			MonthList = Enumerable.Range(1, 12)
				.Select(m => new SelectListItem()
				{
					Value = m.ToString(),
					Text = CultureInfo.CurrentCulture
					.DateTimeFormat.GetMonthName(m)
				})
				.ToList();
			IList<StatusModel> statusList = new List<StatusModel>()
			{
				new ()
				{
					Id = 1,
					Status = "Đang theo dõi",
				},new ()
				{
					Id = 2,
					Status = "Đã hủy",
				},new ()
				{
					Id = 3,
					Status = "Block",
				},
			};
			StatusList = statusList.Select(s => new SelectListItem
			{
				Value = s.Id.ToString(),
				Text = s.Status,
			});
		}

	}
}
