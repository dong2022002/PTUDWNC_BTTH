using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class AuthorRepository : IAuthorRepository
{
	private readonly BlogDbContext _context;
	private readonly IMemoryCache _memoryCache;

	public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache)
	{
		_context = context;
		_memoryCache = memoryCache;
	}

	public async Task<Author> GetAuthorBySlugAsync(
		string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
	}

	public async Task<Author> GetCachedAuthorBySlugAsync(
		string slug, CancellationToken cancellationToken = default)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"author.by-slug.{slug}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetAuthorBySlugAsync(slug, cancellationToken);
			});
	}

	public async Task<Author> GetAuthorByIdAsync(int authorId)
	{
		return await _context.Set<Author>().FindAsync(authorId);
	}

	public async Task<Author> GetCachedAuthorByIdAsync(int authorId)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"author.by-id.{authorId}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetAuthorByIdAsync(authorId);
			});
	}

	public async Task<IList<AuthorItem>> GetAuthorsAsync(
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.OrderBy(a => a.FullName)
			.Select(a => new AuthorItem()
			{
				Id = a.Id,
				FullName = a.FullName,
				Email = a.Email,
				JoinedDate = a.JoinedDate,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
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

	public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
		IPagingParams pagingParams,
		string name = null,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.AsNoTracking()
			.WhereIf(!string.IsNullOrWhiteSpace(name), 
				x => x.FullName.Contains(name))
			.Select(a => new AuthorItem()
			{
				Id = a.Id,
				FullName = a.FullName,
				Email = a.Email,
				JoinedDate = a.JoinedDate,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToPagedListAsync(pagingParams, cancellationToken);
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


	public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
		Func<IQueryable<Author>, IQueryable<T>> mapper,
		IPagingParams pagingParams,
		string name = null,
		CancellationToken cancellationToken = default)
	{
		var authorQuery = _context.Set<Author>().AsNoTracking();

		if (!string.IsNullOrEmpty(name))
		{
			authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
		}

		return await mapper(authorQuery)
			.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<bool> AddOrUpdateAsync(
		Author author, CancellationToken cancellationToken = default)
	{
		if (author.Id > 0)
		{
			_context.Authors.Update(author);
			_memoryCache.Remove($"author.by-id.{author.Id}");
		}
		else
		{
			_context.Authors.Add(author);
		}

		return await _context.SaveChangesAsync(cancellationToken) > 0;
	}
	
	public async Task<bool> DeleteAuthorAsync(
		int authorId, CancellationToken cancellationToken = default)
	{
		return await _context.Authors
			.Where(x => x.Id == authorId)
			.ExecuteDeleteAsync(cancellationToken) > 0;
	}

	public async Task<bool> IsAuthorSlugExistedAsync(
		int authorId, 
		string slug, 
		CancellationToken cancellationToken = default)
	{
		return await _context.Authors
			.AnyAsync(x => x.Id != authorId && x.UrlSlug == slug, cancellationToken);
	}

	public async Task<bool> SetImageUrlAsync(
		int authorId, string imageUrl,
		CancellationToken cancellationToken = default)
	{
		return await _context.Authors
			.Where(x => x.Id == authorId)
			.ExecuteUpdateAsync(x => 
				x.SetProperty(a => a.ImageUrl, a => imageUrl), 
				cancellationToken) > 0;
	}
}