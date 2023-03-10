using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context)
        {
            _context = context;
        }

        public Task BlockSubscriberAsync(int id, string notes, string reason, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default)
        {
           await _context.Set<Subscriber>()
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .Where(s => s.Mail== email)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> SubscriberAsync(string email, CancellationToken cancellationToken = default)
        {
           var newEmail=  await GetSubscriberByEmailAsync (email, cancellationToken);
            if (!(newEmail==null))
            {
                return false;
            }
            _context.Subscribers.Add(new Subscriber
            {
                Mail = email,
                DateRegis= DateTime.Now
            }) ;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

      
        public async Task UnSubscriberAsync(
            string email,
            string reason,
            bool isVoluntary,
            CancellationToken cancellationToken = default)
        {
           await _context.Set<Subscriber>()
               .Where(t => t.Mail == email)
               .ExecuteUpdateAsync(p =>
               p.SetProperty(p => p.IsUserUnFollow, isVoluntary)
               .SetProperty(p => p.Desc, reason)
               .SetProperty(p => p.DateUnFollow, DateTime.Now)
               , cancellationToken);


          

        }
    }
}
