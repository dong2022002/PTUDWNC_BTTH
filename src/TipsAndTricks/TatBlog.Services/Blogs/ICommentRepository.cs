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
	public interface ICommentRepository
	{
		Task<IList<CommentItem>> GetCommentsFromPostIDAsync(
		 int idPost,
		 CancellationToken cancellationToken = default);
		Task<Comment> GetCommentsByIDAsync(int id, CancellationToken cancellationToken = default);
		Task<Comment> AddUpdateCommentAsync(
				Comment comment,
				CancellationToken cancellationToken = default);
		Task<IPagedList<CommentItem>> GetPagedCommentsAsync(
			 CommentQuery condition,
			  int pageNumber = 1,
			  int pageSize = 5,
			  CancellationToken cancellationToken = default);
		Task<bool> SetPublishedComment(
				int idComment,
				CancellationToken cancellationToken = default);

		Task<Comment> DeleteCommentAsync(
			 int id,
			 CancellationToken cancellationToken = default);
        Task<IList<CommentItem>> GetCommentsFromPostQuery(
          CommentQuery query, CancellationToken cancellationToken = default);
    }
}
