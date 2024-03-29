﻿using System;
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
            string name = null,
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

        Task<Category> AddUpdateCategoryAsync(
            Category newTask,
            CancellationToken cancellationToken = default);
        Task<bool> IsCategoryNameExistedAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteCategoryAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
              IPagingParams pagingParams,
              string name =null,
              CancellationToken cancellationToken = default);

        Task<IList<DatePost>> GetPostCountByMonthArchives(
            int month,
            CancellationToken cancellationToken = default);

        Task<int> CountPostsAsync(
        PostQuery condition, CancellationToken cancellationToken = default);
        Task<bool> SetShowOnMenuCategoryAsync(
        int catId,
        CancellationToken cancellationToken = default);


        Task<IPagedList<Post>> GetPagedPostsAsync(
           PostQuery condition,
           int pageNumber = 1,
           int pageSize = 10,
           CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
             CategoryQuery condition,
             int pageNumber = 1,
             int pageSize = 2,
             CancellationToken cancellationToken = default);


        Task<IPagedList<T>> GetPagedPostAsync<T>(
              IPagingParams pagingParams,
              Func<IQueryable<Post>, IQueryable<T>> mapper,
              PostQuery query,
              CancellationToken cancellationToken = default);

        #region post

        Task<Post> GetPostByIdAsync(
           int id,
            bool includeDetails,
           CancellationToken cancellationToken = default);

        Task<bool> AddUpdatePostAsync(
           Post post,
            IEnumerable<string> tags,
           CancellationToken cancellationToken = default);

        Task<bool> SetPublishedPostAsync(
            int PostId,
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

        Task<IPagedList<Post>> GetPagedPostAsync(
                   IPagingParams pagingParams,
                   PostQuery query,
                   CancellationToken cancellationToken = default);


        #endregion

        Task<bool> DeletePostAsync(
         int id,
         CancellationToken cancellationToken = default);
        Task<IList<Statistical>> GetStatistical(CancellationToken cancellationToken = default);

        Task<bool> IsCategorySlugExistedAsync(
            int id,
           string slug,
           CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateCategoryAsync(Category newCat, CancellationToken cancellationToken = default);

        Task<IList<T>> GetFeaturePostMapperAysnc<T>(
          int numberPost,
          Func<IQueryable<Post>, IQueryable<T>> mapper,
          CancellationToken cancellationToken = default
            );

        Task<Post> GetPostBySlugAsync(
            string slug,
             bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<bool> SetPostImageUrlAsync(
            int id, string imageUrl,
            CancellationToken cancellationToken = default);

        Task<Tag> GetTagFromIdAsync(
           int id,
           CancellationToken cancellationToken = default);

        Task<bool> IsTagSlugExitedAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default);
        Task<bool> AddOrUpdateTagAsync(Tag newTag, CancellationToken cancellationToken = default);
	}

}

