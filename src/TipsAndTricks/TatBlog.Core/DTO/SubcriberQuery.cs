using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class SubcriberQuery
	{
		public string Keyword { get; set; }
		public string? Mail { get; set; }
		public int? MonthRegis { get; set; }
		public int? YearRegis { get; set; }
		public bool StatusFollowOnLy { get; set; }
		public bool NotStatusFollow { get; set; }
		public bool IsAdminBlock { get; set;}
	}

}
