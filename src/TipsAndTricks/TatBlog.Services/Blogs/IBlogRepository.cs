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

        Task<int> AddUpdateCategoryAsync(
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

        Task<IList<DatePost>> CountPostMonth(
            int month,
            CancellationToken cancellationToken= default);

        #region post

        Task<Post> GetPostFromIDAsync(
           int id,
           CancellationToken cancellationToken = default);

         Task<int> AddUpdatePostAsync(
            Post newPost,
            CancellationToken cancellationToken = default);

        Task SetPublishedPostAsync(
            bool isPuslished,
            CancellationTokenSource cancellationToken = default);

        Task<IList<Post>> GetPostsRandomAsync(
            int number,
            CancellationToken cancellationToken = default);
        #endregion
    }

}

