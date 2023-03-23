using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class SubscriberEditModel
	{
		public int Id { get; set; }

		[DisplayName("Lý do hủy")]
		public string? Desc { get; set; }

		[DisplayName("Ghi chú của quản trị viên")]
		public string? NoteAdmin { get; set; }

	}
}
