using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public DateTime DateRegis { get; set; }
        public DateTime DateUnFollow { get; set; }
        public string Desc { get; set; }
        public bool IsUserUnFollow { get; set; }
        public string NoteAdmin { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
