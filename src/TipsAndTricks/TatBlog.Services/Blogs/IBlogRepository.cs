using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);
        Task<bool> IsPostSlugExitedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);
        Task<bool> IsCatSlugExitedAsync(
          int catId, string slug,
          CancellationToken cancellationToken = default);
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task<Tag> GetTagFromSlugAsync(
            string slug,
            CancellationToken cancellationToken = default);
		Task<Author> GetAuthorFromSlugAsync(
			string slug,
			CancellationToken cancellationToken = default);
         Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);

		Task<Category> GetCategoryFromSlugAsync(
         string slug,
         CancellationToken cancellationToken = default);

        Task<Category> GetCategoryFromIDAsync(
           int id,
           CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetTagItemListAsync(
            CancellationToken cancellationToken = default);

        Task<bool> delTagAsync(
            int id,
            CancellationToken token = default);

        Task<Category> AddUpdateCategoryAsync(
            Category newTask,
            CancellationToken cancellationToken = default);
        Task<bool> IsCategoryNameExistedAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<bool > DeleteCategoryAsync(
            int id,
            CancellationToken cancellationToken = default);

         Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
               IPagingParams pagingParams,
               CancellationToken cancellationToken = default);

        Task<IList<DatePost>> CountPostMonthAysnc(
            int month,
            CancellationToken cancellationToken= default);

        Task<int> CountPostsAsync(
        PostQuery condition, CancellationToken cancellationToken = default);



		Task<IPagedList<Post>> GetPagedPostsAsync(
           PostQuery condition,
           int pageNumber = 1,
           int pageSize = 10,
           CancellationToken cancellationToken = default);


		Task<IPagedList<T>> GetPagedListPostFromQueryableAsync<T>(
              IPagingParams pagingParams,
              Func<IQueryable<Post>,IQueryable<T>> mapper,
              PostQuery query,
              CancellationToken cancellationToken = default);

        #region post

        Task<Post> GetPostByIdAsync(
           int id,
			bool includeDetails,
		   CancellationToken cancellationToken = default);

         Task<Post> AddUpdatePostAsync(
            Post post,
			 IEnumerable<string> tags,
			CancellationToken cancellationToken = default);

        Task<bool> SetPublishedPostAsync(
            int isPuslished,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> GetPostsRandomAsync(
            int number,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> GetFeaturePostAysnc(
            int numberPost,
            CancellationToken cancellationToken = default);
        #endregion

        #region PostQuery
        Task<IList<Post>> GetPostsFromPostQuery(
            PostQuery query, CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPagedListPostFromPostQueryAsync(
                   IPagingParams pagingParams,
                   PostQuery query,
                   CancellationToken cancellationToken = default);
      
            
        #endregion
    }
   

}

