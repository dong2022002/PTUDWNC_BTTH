using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class PostEditModel
	{
        public PostEditModel()
        {
            
        }

        public int Id { get; set; }

		[DisplayName("Tiêu đề")]
		public string? Title { get; set; }

		[DisplayName("Giới thiệu")]
		public string? ShortDescription { get; set; }

		[DisplayName("Nội dung")]
		public string? Description { get; set; }

		[DisplayName("Metadata")]
		public string? Meta { get; set; }

		[DisplayName("Slug")]
		[Remote("VerifyPostSlug","Posts","Admin",
			HttpMethod = "POST", AdditionalFields ="Id")]
		public string? UrlSlug { get; set; }

		[DisplayName("Chọn hình ảnh")]
		public IFormFile? ImageFile { get; set; }

		[DisplayName("Hình hiện tại")]
		public string? ImageUrl{ get; set; }

        public bool Published { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public string? SelectedTags { get; set; }
        public IEnumerable<SelectListItem>? AuthorList { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

		public List<string> GetSelectedTags()
		{
			return (SelectedTags ?? "")
				.Split(new[] {',',';','\r','\n'},
				StringSplitOptions.RemoveEmptyEntries)
				.ToList();
		}
    }
}
