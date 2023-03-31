using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using TatBlog.Services.Media;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs
{
	public class BlogRepository : IBlogRepository
	{
		private readonly BlogDbContext _context;
		private readonly IMediaManager _mediaManager;

		public BlogRepository(BlogDbContext context, IMediaManager mediaManager)
		{
			_context = context;
			_mediaManager = mediaManager;	
		}



		public async Task<IList<CategoryItem>> GetCategoriesAsync(
			bool showOnMenu = false,
			CancellationToken cancellationToken = default)
		{
			IQueryable<Category> categories = _context.Set<Category>();

			if (showOnMenu)
			{
				categories = categories.Where(x => x.ShowOnMenu);
			}
			return await categories
				.OrderBy(x => x.Name)
				.Select(x => new CategoryItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					ShowOnMenu = x.ShowOnMenu,
					PostCount = x.Posts.Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}



		public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				 .Include(x => x.Author)
				 .Include(x => x.Category)
				 .OrderByDescending(p => p.ViewCount)
				 .Take(numPosts)
				 .ToListAsync(cancellationToken);

		}

		public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = false,
				MonthPost = month,
				YearPost = year,
				TitleSlug = slug
			};

			return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken); ;
		}
		public async Task<Post> GetPostAsync(
		string slug,
		CancellationToken cancellationToken = default)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = false,
				TitleSlug = slug
			};

			return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
		}






		public async Task IncreaseViewCountAsync(
			int postId,
			CancellationToken cancellationToken = default)
		{
			await _context.Set<Post>()
				 .Where(x => x.Id == postId)
				 .ExecuteUpdateAsync(p =>
					 p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
					 cancellationToken);
		}

		public async Task<bool> IsPostSlugExitedAsync(
			int postId,
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
				cancellationToken);
		}


		public async Task<bool> IsTagSlugExitedAsync(
			int id,
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.AnyAsync(x => x.Id != id && x.UrlSlug == slug,
				cancellationToken);
		}

		#region Tag
		public async Task<IList<TagItem>> GetTagItemListAsync(CancellationToken cancellationToken = default)
		{
			IQueryable<Tag> tagItems = _context.Set<Tag>();

			return await tagItems
				.Select(x => new TagItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					PostCount = x.Posts.Count(p => p.Published)
				})
			.ToListAsync(cancellationToken);
		}
		public async Task<Tag> GetTagFromSlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
			   .Where(t => t.UrlSlug == slug)
			   .FirstOrDefaultAsync(cancellationToken);

		}

		public async Task<Tag> GetTagFromIdAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
			   .Where(t => t.Id == id)
			   .FirstOrDefaultAsync(cancellationToken);

		}

		public async Task<bool> delTagAsync(int id, CancellationToken cancellationToken = default)
		{
			var tag = _context.Set<Tag>()
			   .Where(t => t.Id == id);
			if (tag == null) return false;
			await tag.ExecuteDeleteAsync(cancellationToken);
			return true;

		}
		#endregion

		#region Category
		public async Task<Category> GetCategoryFromSlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
			  .Where(c => c.UrlSlug == slug)
			  .FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<Category> GetCategoryFromIDAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
			.Include(p => p.Posts)
			.Where(c => c.Id == id)
			.FirstOrDefaultAsync(cancellationToken);

		}

		public async Task<Category> AddUpdateCategoryAsync(Category newCat, CancellationToken cancellationToken = default)
		{
			if (newCat.Id <= 0)
			{
				_context.Categories.Add(newCat);

			}
			else
			{
				_context.Set<Category>().Update(newCat);

			}
			await _context.SaveChangesAsync(cancellationToken);
			return newCat;
		}

		public async Task<bool> AddOrUpdateCategoryAsync(Category newCat, CancellationToken cancellationToken = default)
		{
			if (newCat.Id <= 0)
			{
				_context.Categories.Add(newCat);

			}
			else
			{
				_context.Set<Category>().Update(newCat);

			}
			return (await _context.SaveChangesAsync(cancellationToken)) > 0;
		
		}

		public async Task<bool> AddOrUpdateTagAsync(Tag newTag, CancellationToken cancellationToken = default)
		{
			if (newTag.Id <= 0)
			{
				_context.Tags.Add(newTag);

			}
			else
			{
				_context.Set<Tag>().Update(newTag);

			}
			return (await _context.SaveChangesAsync(cancellationToken)) > 0;

		}

		public async Task<bool> IsCategoryNameExistedAsync(
			string name,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.AnyAsync(x => x.Name == name, cancellationToken);
		}

		public async Task<bool> IsCategorySlugExistedAsync(
			int id ,
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
		}


		public async Task<bool> DeleteCategoryAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			var cat = _context.Set<Category>()
			  .Where(t => t.Id == id);
			if (cat.IsNullOrEmpty()) return false;
			await cat.ExecuteDeleteAsync(cancellationToken);
			return true;
		}


		public async Task<bool> DeletePostAsync(
			int id,

			CancellationToken cancellationToken = default)
		{
			var post = _context.Set<Post>()
			  .Where(t => t.Id == id);
			if (post.IsNullOrEmpty()) return false;
			
			var postData = await post.FirstOrDefaultAsync(cancellationToken);
			await _mediaManager.DeleteFileAsync(postData.ImageUrl, cancellationToken);
			await post.ExecuteDeleteAsync(cancellationToken);

			return true;
		}
		public async Task<bool> IsCatSlugExitedAsync(
			int catId,
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
						   .AnyAsync(x => x.Id != catId && x.UrlSlug == slug,
						   cancellationToken);
		}
		#endregion

		#region PageList
		public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
		 IPagingParams pagingParams,
		 string name = null,
		 CancellationToken cancellationToken = default)
		{
			var tagQuery = _context.Set<Tag>()
					.WhereIf(!string.IsNullOrWhiteSpace(name),
				x => x.Name.Contains(name))
				.Select(x => new TagItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					PostCount = x.Posts.Count(p => p.Published)
				});
			return await tagQuery
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

	
		public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
		 IPagingParams pagingParams,
		 string name = null,
		 CancellationToken cancellationToken = default)
		{
			var catQuery = _context.Set<Category>()
				.WhereIf(!string.IsNullOrWhiteSpace(name),
				x => x.Name.Contains(name))
				.Select(x => new CategoryItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					PostCount = x.Posts.Count(p => p.Published)
				});
			return await catQuery
				.ToPagedListAsync(pagingParams, cancellationToken);
		}
		public async Task<IPagedList<Post>> GetPagedPostsAsync(
			 PostQuery condition,
			 int pageNumber = 1,
			 int pageSize = 2,
			 CancellationToken cancellationToken = default)
		{
			return await FilterPosts(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Post.PostedDate), "DESC",
				cancellationToken);

		}
		public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
			 CategoryQuery condition,
			 int pageNumber = 1,
			 int pageSize = 2,
			 CancellationToken cancellationToken = default)
		{
			return await FilterCategories(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(CategoryItem.Name), "ASC",
				cancellationToken);

		}

	

		public async Task<IPagedList<Post>> GetPagedPostAsync(
			   IPagingParams pagingParams,
			   PostQuery query,
			   CancellationToken cancellationToken = default)
		{
			var postQuery = FilterPosts(query);
			return await postQuery
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<IPagedList<T>> GetPagedPostAsync<T>(
			 IPagingParams pagingParams,
			 Func<IQueryable<Post>, IQueryable<T>> mapper,
			 PostQuery query,
			 CancellationToken cancellationToken = default)
		{
			var postQuery = FilterPosts(query);

			return await mapper(postQuery)
				 .ToPagedListAsync(pagingParams, cancellationToken);
		}

		
		#endregion

		#region Count
		public async Task<IList<DatePost>> GetPostCountByMonthArchives(
			int numberMonth,
			CancellationToken cancellationToken = default)
		{
			var now = DateTime.Now;
			return await _context.Set<Post>()
				.Where(p => p.Published)
				.GroupBy(
				p => new
				{
					p.PostedDate.Month,
					p.PostedDate.Year
				},
				(key, g) => new DatePost()
				{
					Month = key.Month,
					Year = key.Year,
					PostCount = g.Count()
				})
				.OrderByDescending(p => p.Year)
				.ThenByDescending(p => p.Month)
				.Take(numberMonth)
				.ToListAsync(cancellationToken);


		}
		public async Task<int> CountPostsAsync(
		PostQuery condition, CancellationToken cancellationToken = default)
		{
			return await FilterPosts(condition).CountAsync(cancellationToken: cancellationToken);
		}



		#endregion

		#region Post

		public async Task<Post> GetPostByIdAsync(
			int id,
			 bool includeDetails = false,
			CancellationToken cancellationToken = default)
		{
			if (!includeDetails)
			{
				return await _context.Set<Post>().FindAsync(id);
			}

			return await _context.Set<Post>()
				.Include(x => x.Tags)
				.Include(x => x.Category)
				.Include(x => x.Author)
				.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}

		public async Task<Post> GetPostBySlugAsync(
			string slug,
			 bool includeDetails = false,
			CancellationToken cancellationToken = default)
		{
			if (!includeDetails)
			{
				return await _context.Set<Post>().FindAsync(slug);
			}

			return await _context.Set<Post>()
				.Include(x => x.Tags)
				.Include(x => x.Category)
				.Include(x => x.Author)
				.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
		}

		public async Task<bool> AddUpdatePostAsync(
			Post post,
			IEnumerable<string> tags,
			CancellationToken cancellationToken = default)
		{
			if (post.Id > 0)
			{
				await _context
					.Entry(post)
					.Collection(x => x.Tags)
					.LoadAsync(cancellationToken);
			}
			else
			{
				post.Tags = new List<Tag>();
			}

			var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => new
				{
					Name = x,
					Slug = x.GenerateSlug()
				})
				.GroupBy(x => x.Slug)
				.ToDictionary(g => g.Key, g => g.First().Name);


			foreach (var kv in validTags)
			{
				if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

				var tag = await GetTagFromSlugAsync(kv.Key, cancellationToken) ?? new Tag()
				{
					Name = kv.Value,
					Description = kv.Value,
					UrlSlug = kv.Key
				};

				post.Tags.Add(tag);
			}

			post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

			if (post.Id > 0)
				_context.Update(post);
			else
				_context.Add(post);
			return await _context.SaveChangesAsync(cancellationToken) > 0;

		}

		public async Task<bool> SetPublishedPostAsync(
		 int postId,
		 CancellationToken cancellationToken = default)
		{
			var post = await _context.Set<Post>().FindAsync(postId);

			if (post is null) return false;

			post.Published = !post.Published;

			await _context.SaveChangesAsync(cancellationToken);

			return post.Published;
		}
		public async Task<bool> SetShowOnMenuCategoryAsync(
		 int catId,
		 CancellationToken cancellationToken = default)
		{
			var cat = await _context.Set<Category>().FindAsync(catId);

			if (cat is null) return false;

			cat.ShowOnMenu = !cat.ShowOnMenu;

			await _context.SaveChangesAsync(cancellationToken);

			return cat.ShowOnMenu;
		}

		public async Task<IList<Post>> GetPostsRandomAsync(
		  int number,
		  CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
			.OrderBy(x => Guid.NewGuid())
			.Take(number)
			.ToListAsync(cancellationToken);
		}
		#endregion

		#region PostQuery
		public async Task<IList<Post>> GetPostsFromPostQuery(
			PostQuery query, CancellationToken cancellationToken = default)
		{
			query.Count = 0;
			IQueryable<Post> data = FilterPosts(query);

			query.Count = await data.CountAsync(cancellationToken);
			return await data.ToListAsync(cancellationToken);
		}
		
		public async Task<IList<Statistical>> GetStatistical(CancellationToken cancellationToken = default)
		{

			var post = await FilterPosts(new PostQuery()
			{
				PublishedOnly = false,
			}).CountAsync(cancellationToken);
            var NotPublishedPost = await FilterPosts(new PostQuery()
            {
                NotPublished = true,
            }).CountAsync(cancellationToken);

			var cat = await _context.Set<Category>().CountAsync(cancellationToken);
			var author = await _context.Set<Author>().CountAsync(cancellationToken);
			var comment = await _context.Set<Comment>()
					.Where(c => !c.Published)
					.CountAsync(cancellationToken);
			var subscriber = await _context.Set<Subscriber>().CountAsync(cancellationToken);


            IList<Statistical> list = new List<Statistical>()
            {
                new Statistical()
                {
                    Title = "Tổng số bài viết",
                    Count = post,
                },
                 new Statistical()
                {
                    Title = "Số bài viết chưa xuất bản",
                    Count = NotPublishedPost,
                },
                 new Statistical()
                {
                    Title = "Tổng số chủ đề",
                    Count = cat,
                },
                   new Statistical()
                {
                    Title = "Tổng số tác giả",
                    Count = author,
                },
                      new Statistical()
                {
                    Title = "Tổng số Bình luận chờ duyệt",
                    Count = comment,
                },
                           new Statistical()
                {
                    Title = "Tổng số người theo dõi",
                    Count = subscriber,
                },
            };
			return list;
		}



        #endregion

        #region Author
        public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
		{
			return await _context.Set<Author>()
				.OrderBy(a => a.FullName)
				.Select(a => new AuthorItem()
				{
					Id = a.Id,
					FullName = a.FullName,
					Email = a.ToString(),
					JoinedDate = a.JoinedDate,
					ImageUrl = a.ImageUrl,
					UrlSlug = a.UrlSlug,
					Notes = a.Notes,
					PostCount = a.Posts.Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}
		#endregion
		private IQueryable<Post> FilterPosts(PostQuery query)
		{
			IQueryable<Post> posts = _context.Set<Post>()
				.Include(a => a.Author)
				.Include(a => a.Category)
				.Include(a => a.Tags);

			if (query.TitleSlug != null)
			{
				posts = posts.Where(x => x.UrlSlug == query.TitleSlug);
			}
			if (query.PublishedOnly)
			{
				posts = posts.Where(x => x.Published);
			}
			if (query.NotPublished)
			{
				posts = posts.Where(x => !x.Published);
			}

			if (query.CategoryId > 0)
			{
				posts = posts.Where(x => x.CategoryId == query.CategoryId);
			}

			if (query.AuthorId > 0)
			{
				posts = posts.Where(x => x.AuthorId == query.AuthorId);
			}
			if (!query.AuthorSlug.IsNullOrEmpty())
			{
				posts = posts
					 .Where(p =>
					 (p.Author.UrlSlug == query.AuthorSlug));
			}
			if (!query.CategorySlug.IsNullOrEmpty())
			{
				posts = posts
						 .Where(p =>
							(p.Category.UrlSlug == query.CategorySlug));
			}
			if (!query.TagSlug.IsNullOrEmpty())
			{
				posts = posts
						 .Where(p =>
							(p.Tags.Any(t => t.UrlSlug.Equals(query.TagSlug))));
			}
			if (!query.Keyword.IsNullOrEmpty())
			{
				posts = posts.Where(x => x.Title.Contains(query.Keyword) ||
										 x.ShortDescription.Contains(query.Keyword) ||
										 x.Description.Contains(query.Keyword) ||
										 x.Category.Name.Contains(query.Keyword) ||
										 x.Tags.Any(t => t.Name.Contains(query.Keyword)));
			}
			if (query.YearPost > 0)
			{
				posts = posts.Where(x => x.PostedDate.Year == query.YearPost);
			}

			if (query.MonthPost > 0)
			{
				posts = posts.Where(x => x.PostedDate.Month == query.MonthPost);
			}


			return posts;

		}

		private IQueryable<CategoryItem> FilterCategories(CategoryQuery condition)
		{
			IQueryable<CategoryItem> cats = _context.Set<Category>()
				.Select(x => new CategoryItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					ShowOnMenu = x.ShowOnMenu,
					PostCount = x.Posts.Count(p => p.Published)
				});

			if (!condition.keyword.IsNullOrEmpty())
			{
				cats = cats.Where(x => x.Name.Contains(condition.keyword) ||
										 x.Description.Contains(condition.keyword) );
			}
			if (condition.Name != null)
			{
				cats = cats.Where(x => x.Name == condition.Name);
			}
			if (condition.UrlSlug != null)
			{
				cats = cats.Where(x => x.UrlSlug == condition.UrlSlug);
			}
			if (condition.isShowOnMenu)
			{
				cats = cats.Where(x => x.ShowOnMenu);
			}
			return cats;
		}

		public async Task<Author> GetAuthorFromSlugAsync(
			string slug,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Author>()
			  .Where(t => t.UrlSlug == slug)
			  .FirstOrDefaultAsync(cancellationToken);
		}


		public async Task<IList<T>> GetFeaturePostMapperAysnc<T>(
		  int numberPost,
		  Func<IQueryable<Post>, IQueryable<T>> mapper,
		  CancellationToken cancellationToken = default
			)
		{

			var posts = _context.Set<Post>()
				.Include(x => x.Category)
				.Include(x => x.Author)
				.Include(x => x.Tags)
				.OrderByDescending(x => x.ViewCount)
				.Take(numberPost);
			return await mapper(posts).ToListAsync(cancellationToken);
				
		}
		public async Task<IList<Post>> GetFeaturePostAysnc(int numberPost, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>().OrderByDescending(x => x.ViewCount)
				.Take(numberPost).ToListAsync(cancellationToken);
		}

		public async Task<bool> SetPostImageUrlAsync(
		int id, string imageUrl,
		CancellationToken cancellationToken = default)
		{
			return await _context.Posts
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(x =>
					x.SetProperty(a => a.ImageUrl, a => imageUrl),
					cancellationToken) > 0;
		}

	}
}

