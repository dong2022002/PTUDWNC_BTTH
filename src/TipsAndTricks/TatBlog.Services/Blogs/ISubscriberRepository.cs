using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ISubscriberRepository
    {

        Task<bool> SubscriberAsync(
            string email,
            CancellationToken cancellationToken =default);
        Task UnSubscriberAsync(
        string email,
        string reason,
        bool isVoluntary,
        CancellationToken cancellationToken = default);

        Task BlockSubscriberAsync(
           int id,
       string notes,
       string reason,
       CancellationToken cancellationToken = default);
        Task DeleteSubscriberAsync(
           int id,
       CancellationToken cancellationToken = default);
        Task<Subscriber>  GetSubscriberByIdAsync(
          int id,
      CancellationToken cancellationToken = default);
        Task<Subscriber> GetSubscriberByEmailAsync(
      string email,
        CancellationToken cancellationToken = default);
    }
}
