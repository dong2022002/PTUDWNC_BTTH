using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class CategoryQuery
	{
		public string keyword { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public bool isShowOnMenu { get; set; }
	}
}
