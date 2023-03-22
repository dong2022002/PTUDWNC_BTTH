using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
	public class Comment
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Published { get; set; }

        public DateTime DateComment { get; set; }
        public string Description { get; set; }
		public int PostId { get; set; }

		public Post Post { get; set; }

	}
}
