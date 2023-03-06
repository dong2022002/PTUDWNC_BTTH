using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int AuthorId { get; set; }
        public int CatgoryId { get; set; }
        public int MonthPost { get; set; }
        public int YearPost { get; set; }
        public int TagId { get; set; }
        public int Count { get; set; }
    }
}
