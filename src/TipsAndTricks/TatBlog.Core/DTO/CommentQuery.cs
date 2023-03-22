using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class CommentQuery
	{
		public string Keyword { get; set; }
		public string Name { get; set; }
		public bool PublishedOnly { get; set; }
		public bool NotPublished { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
		public string PostSlug { get; set; }
		public int? PostId { get; set; }
	}
}
