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
	public interface ICommentRepository
	{
		Task<IList<CommentItem>> GetCommentsFromPostIDAsync(
		 int idPost,
		 int number,
		 CancellationToken cancellationToken = default);
		Task<Comment> GetCommentsPostIDAsync(int id, CancellationToken cancellationToken = default);
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

		Task<Comment> DeleteAuthorAsync(
			 int id,
			 CancellationToken cancellationToken = default);
	}
}
