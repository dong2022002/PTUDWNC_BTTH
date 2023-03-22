using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
	public class CommentItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Published { get; set; }
		public DateTime DateComment { get; set; }
		public string Description { get; set; }
		public string PostName { get; set; }

		public int PostId { get;set; }
	}
}
