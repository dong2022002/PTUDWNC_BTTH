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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
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
            IQueryable<Post> postQuery = _context.Set<Post>()
               .Include(x => x.Category)
               .Include(x => x.Author);

            if (year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == month);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == slug);
            }

            return await postQuery.FirstOrDefaultAsync(cancellationToken);
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
              .Where(c => c.Id == id)
              .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> AddUpdateCategoryAsync(Category newCat, CancellationToken cancellationToken = default)
        {
            if (newCat.Id <= 0)
            {
                _context.Categories.Add(newCat);
                await _context.SaveChangesAsync(cancellationToken);
                return 1;
            }
            else
            {
                await _context.Set<Category>()
                   .Where(x => x.Id == newCat.Id)
                   .ExecuteUpdateAsync(t =>
                       t.SetProperty(x => x.Name, newCat.Name)
                       .SetProperty(x => x.ShowOnMenu, newCat.ShowOnMenu)
                       .SetProperty(x => x.UrlSlug, newCat.UrlSlug)
                       .SetProperty(x => x.Description, newCat.Description),
                       cancellationToken); ;
                return 2;
            }
        }

        public async Task<bool> IsCategoryNameExistedAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> DeleteCategoryAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var cat = _context.Set<Category>()
              .Where(t => t.Id == id);
            if (cat == null) return false;
            await cat.ExecuteDeleteAsync(cancellationToken);
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
         CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
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
         CancellationToken cancellationToken = default)
        {
            var catQuery = _context.Set<Category>()
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

        public async Task<IPagedList<Post>> GetPagedListPostFromPostQueryAsync(
               IPagingParams pagingParams,
               PostQuery query,
               CancellationToken cancellationToken = default)
        {
            var postQuery = FilterPosts(query);
            return await postQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedListPostFromQueryableAsync<T>(
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
        public async Task<IList<DatePost>> CountPostMonth(
            int month,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;
            return await _context.Set<Post>()
                .Where(p => p.Published)
                .GroupBy(
                p => new {
                    p.PostedDate.Month,
                    p.PostedDate.Year },
                (key, g) => new DatePost()
                {
                    Month = key.Month,
                    Year = key.Year,
                    PostCount = g.Count()
                })
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .Take(month)
                .ToListAsync(cancellationToken);


        }



        #endregion

        #region Post

        public async Task<Post> GetPostFromIDAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
              .Where(c => c.Id == id)
              .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> AddUpdatePostAsync(
            Post newPost,
            CancellationToken cancellationToken = default)
        {
            if (newPost.Id <= 0)
            {
                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync(cancellationToken);
                return 1;
            }
            else
            {

                await _context.Set<Post>()
                           .Where(x => x.Id == newPost.Id)
                           .ExecuteUpdateAsync(t =>
                               t.SetProperty(x => x.Title, newPost.Title)
                               .SetProperty(x => x.ShortDescription, newPost.ShortDescription)
                               .SetProperty(x => x.Description, newPost.Description)
                               .SetProperty(x => x.Meta, newPost.Meta)
                               .SetProperty(x => x.UrlSlug, newPost.UrlSlug)
                               .SetProperty(x => x.ImageUrl, newPost.ImageUrl)
                               .SetProperty(x => x.ViewCount, newPost.ViewCount)
                               .SetProperty(x => x.Published, newPost.Published)
                               .SetProperty(x => x.PostedDate, newPost.PostedDate)
                               .SetProperty(x => x.ModifiedDate, newPost.ModifiedDate)
                               .SetProperty(x => x.Author, newPost.Author)
                               .SetProperty(x => x.Category, newPost.Category)
                               .SetProperty(x => x.Tags, newPost.Tags),
                               cancellationToken);
                return 2;
            }
        }

        public async Task SetPublishedPostAsync(
         bool isPuslished,
         CancellationTokenSource cancellationToken = default)
        {
            await _context.Set<Post>()
                .ExecuteUpdateAsync(p =>
                p.SetProperty(x => x.Published, isPuslished));
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




        #endregion
        private IQueryable<Post> FilterPosts(PostQuery query)
		{


			return  _context.Set<Post>()
					.Include(a => a.Author)
					.Include(c => c.Category)
					.Include(t => t.Tags)
						.Where(p =>
                        (p.Author.UrlSlug == query.AuthorSlug || query.AuthorSlug.IsNullOrEmpty()) &&
						(p.Category.UrlSlug == query.CategorySlug || query.CategorySlug.IsNullOrEmpty() &&
						(p.PostedDate.Month == query.MonthPost || query.MonthPost == 0) &&
						(p.PostedDate.Year == query.YearPost || query.YearPost == 0) &&
                        (p.Published == query.PublishedOnly)));

		}

		

	}
}

