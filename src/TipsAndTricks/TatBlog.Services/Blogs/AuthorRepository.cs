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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
		   AuthorQuery condition,
			 int pageNumber = 1,
			 int pageSize = 2,
			 CancellationToken cancellationToken = default)
        {
          
			return await FilterAuthors(condition).ToPagedListAsync(
			 pageNumber, pageSize,
			 nameof(AuthorItem.FullName), "ASC",
			 cancellationToken);
		}

		private IQueryable<AuthorItem> FilterAuthors(AuthorQuery condition)
		{
			var authors = _context.Set<Author>()
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
			if (condition.FullName != null)
			{
				authors = authors.Where(x => x.FullName == condition.FullName);
			}
		
			if (!condition.keyword.IsNullOrEmpty())
			{
                authors = authors.Where(x => x.FullName.Contains(condition.keyword) ||
                                         x.Email.Contains(condition.keyword) ||
                                         x.Notes.Contains(condition.keyword));
				
			}
			if (condition.Year > 0)
			{
				authors = authors.Where(x => x.JoinedDate.Year == condition.Year);
			}

			if (condition.Month > 0)
			{
				authors = authors.Where(x => x.JoinedDate.Month == condition.Month);
			}
            return authors;
		}

		public async Task<Author> AddUpdateAuthorAsync(
            Author author,
            CancellationToken cancellationToken = default)
        {
			if (author.Id <= 0)
			{
				_context.Authors.Add(author);

			}
			else
			{
				_context.Set<Author>().Update(author);

			}
			await _context.SaveChangesAsync(cancellationToken);
			return author;
		}

        public async Task<IList<AuthorItem>> GetNumberAuthorItemsAsync(
            int number,
            CancellationToken cancellation = default)
        {
            return await  _context.Set<Author>()
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
             })
             .OrderByDescending(x => x.PostCount)
             .Take(number)
             .ToListAsync(cancellation);

        }
		public async Task<Author> DeleteAuthorAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			var author = _context.Set<Author>()
			  .Where(t => t.Id == id);
			if (author == null) return null;
			var authorData = await author.FirstOrDefaultAsync(cancellationToken);
			await author.ExecuteDeleteAsync(cancellationToken);
			return authorData;
		}
	}
}
