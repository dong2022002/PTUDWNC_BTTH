using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task BlockSubscriberAsync(int id, string notes, string reason, CancellationToken cancellationToken = default)
        {
            await _context.Set<Subscriber>()
               .Where(t => t.Id == id)
               .ExecuteUpdateAsync(p =>
               p.SetProperty(p => p.IsUserUnFollow, false)
               .SetProperty(p => p.Desc, reason)
               .SetProperty(p => p.StatusFollow, false)
               .SetProperty(p => p.DateUnFollow, DateTime.Now)
               .SetProperty(p => p.NoteAdmin, notes)
               , cancellationToken);

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
                if ((newEmail.StatusFollow == false))
                {
					await _context.Set<Subscriber>()
			              .Where(t => t.Mail == email)
			              .ExecuteUpdateAsync(p =>
			              p.SetProperty(p => p.IsUserUnFollow, null)
			              .SetProperty(p => p.Desc, "")                         
			              .SetProperty(p => p.StatusFollow, true)
			              .SetProperty(p => p.DateUnFollow, new DateTime())
			              , cancellationToken);}
            }
            _context.Subscribers.Add(new Subscriber
            {
                Mail = email,
                DateRegis= DateTime.Now,
                StatusFollow = true,
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
               .SetProperty(p=> p.StatusFollow, false)
               .SetProperty(p => p.DateUnFollow, DateTime.Now)
               , cancellationToken);

        }
		public async Task<IPagedList<Subscriber>> GetPagedSubcriberAsync(
            SubcriberQuery condition,
            int pageNumber = 1,
            int pageSize = 5,
            CancellationToken cancellationToken = default)
		{
			return await FilterSubcriber(condition).ToPagedListAsync(
			 pageNumber, pageSize,
			 nameof(Subscriber.DateRegis), "DESC",
			 cancellationToken);
		}

		private IQueryable<Subscriber> FilterSubcriber(SubcriberQuery condition)
		{
            IQueryable<Subscriber> subcriber = _context.Set<Subscriber>();

			if (condition.Mail != null)
			{
				subcriber = subcriber.Where(x => x.Mail == condition.Mail);
			}

			if (!condition.Keyword.IsNullOrEmpty())
			{
                subcriber = subcriber.Where(x => x.Mail.Contains(condition.Keyword) ||
                                        x.Desc.Contains(condition.Keyword) ||
                                        x.NoteAdmin.Contains(condition.Keyword));

			}
			if (condition.YearRegis > 0)
			{
				subcriber = subcriber.Where(x => x.DateRegis.Year == condition.YearRegis);
			}

			if (condition.MonthRegis > 0)
			{
				subcriber = subcriber.Where(x => x.DateRegis.Month == condition.YearRegis);
			}
			if (condition.StatusFollowOnLy)
			{
				subcriber = subcriber.Where(x => x.StatusFollow);
			}
			if (condition.IsAdminBlock)
			{
				subcriber = subcriber.Where(x => !x.StatusFollow && !x.IsUserUnFollow);
			}
			if (condition.NotStatusFollow)
			{
				subcriber = subcriber.Where(x => !x.StatusFollow && x.IsUserUnFollow);
			}
			return subcriber;
		}
	}
}
