using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Blogs
{
    public interface ISubscriberRepository
    {

        Task SubscriberAsync(
            int postId,
            string email,
            CancellationToken cancellationToken =default);
        Task UnSubscriberAsync(
            int postId,
        string email,
        string reason,
        bool isVoluntary,
        CancellationToken cancellationToken = default);
    }
}
