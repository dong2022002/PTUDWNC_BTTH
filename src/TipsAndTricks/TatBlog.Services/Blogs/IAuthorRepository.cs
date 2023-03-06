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
    public interface IAuthorRepository
    {
        Task<Author> GetAuthorFromIDAsync(
          int id,
          CancellationToken cancellationToken = default);

        Task<Author> GetAuthorFromSlugAsync(
           string slug,
           CancellationToken cancellationToken = default);

        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task<int> AddUpdateAuthorAsync(
                Author author,
                CancellationToken cancellationToken = default);

        Task<IList<AuthorItem>> GetNumberAuthorItemsAsync(
            int number,
            CancellationToken cancellation = default);
    }
}
