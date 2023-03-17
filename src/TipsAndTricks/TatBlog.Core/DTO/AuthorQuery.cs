using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class AuthorQuery
	{
		public string keyword { get; set; }
		public string FullName { get; set; }
		public string UrlSlug { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public string Email { get; set; }
	}
}