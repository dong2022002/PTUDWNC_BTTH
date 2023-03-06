using Microsoft.EntityFrameworkCore;
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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;

        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
        }

        

        public async Task<Author> GetAuthorFromIDAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
             .Where(c => c.Id == id)
             .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Author> GetAuthorFromSlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
               .Where(t => t.UrlSlug == slug)
               .FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams, 
            CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Author>()
               .Select(x => new AuthorItem()
               {
                   Id = x.Id,
                   FullName = x.FullName,
                   UrlSlug = x.UrlSlug,
                   ImageUrl = x.ImageUrl,
                   JoinedDate = x.JoinedDate,
                   Email = x.Email,
                   Notes = x.Notes,
                   PostCount = x.Posts.Count(p => p.Published)
               });
            return await authorQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<int> AddUpdateAuthorAsync(
            Author author,
            CancellationToken cancellationToken = default)
        {
            if (author.Id <= 0)
            {
                _context.Authors.Add(author);
                await _context.SaveChangesAsync(cancellationToken);
                return 1;
            }
            else
            {

                await _context.Set<Author>()
                           .Where(x => x.Id == author.Id)
                           .ExecuteUpdateAsync(t =>
                               t.SetProperty(x => x.FullName, author.FullName)
                               .SetProperty(x => x.UrlSlug, author.UrlSlug)
                               .SetProperty(x => x.ImageUrl, author.UrlSlug)
                               .SetProperty(x => x.JoinedDate, author.JoinedDate)
                               .SetProperty(x => x.Email, author.Email)
                               .SetProperty(x => x.Notes, author.Notes),
                               cancellationToken);
                return 2;
            }
        }
    }
}
