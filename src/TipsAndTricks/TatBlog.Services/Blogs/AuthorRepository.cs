using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

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
    }
}
