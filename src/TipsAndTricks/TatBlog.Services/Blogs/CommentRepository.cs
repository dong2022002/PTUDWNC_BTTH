using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
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
	public class CommentRepository : ICommentRepository
	{
		private readonly BlogDbContext _context;

		public CommentRepository(BlogDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AddUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
		{
			if (comment.Id <= 0)
			{
				_context.Comments.Add(comment);

			}
			else
			{
				_context.Set<Comment>().Update(comment);

			}

			return
				await _context.SaveChangesAsync(cancellationToken) > 0;
		}

		public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
		{
			var comment = _context.Set<Comment>()
			  .Where(t => t.Id == id);
			if (comment.IsNullOrEmpty()) return false;
			await comment.ExecuteDeleteAsync(cancellationToken);
			return true;
		}

		public async Task<IList<CommentItem>> GetCommentsFromPostIDAsync(int idPost, CancellationToken cancellationToken = default)
		{
			IQueryable<Comment> comments = _context.Set<Comment>()
												.Include(p => p.Post);
			var commentPostId =  comments.Where(c => c.Post.Id == idPost)
										.Select(c => new CommentItem()
										{
											Id = c.Id,
											Name = c.Name,
											Published = c.Published,
											DateComment = c.DateComment,
											Description = c.Description,
											PostName = c.Post.Title,
											PostId = c.Post.Id,
										});
			
				return await commentPostId.ToListAsync(cancellationToken);
		}

		public async Task<Comment> GetCommentsByIDAsync(int id, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Comment>()
		   .Where(c => c.Id == id)
		   .FirstOrDefaultAsync(cancellationToken);
		}

		
		public async Task<IPagedList<CommentItem>> GetPagedCommentsAsync(CommentQuery condition, int pageNumber = 1, int pageSize = 5, CancellationToken cancellationToken = default)
		{
			return await FilterComment(condition).ToPagedListAsync(
			 pageNumber, pageSize,
			 nameof(CommentItem.DateComment), "DESC",
			 cancellationToken);
		}

		private IQueryable<CommentItem> FilterComment(CommentQuery condition)
		{
			var comments = _context.Set<Comment>()
			.Select(c => new CommentItem()
			{
				Id = c.Id,
				Name = c.Name,
				Published = c.Published,
				DateComment = c.DateComment,
				Description = c.Description,
				PostName = c.Post.Title,
				PostId = c.Post.Id
			});
			if (condition.PostId != null)
			{
				comments = comments.Where(x => x.PostId == condition.PostId);
			}

			if (!condition.Keyword.IsNullOrEmpty())
			{
				comments = comments.Where(x => x.Name.Contains(condition.Keyword) ||
										 x.Description.Contains(condition.Keyword));

			}
			if (condition.Year > 0)
			{
				comments = comments.Where(x => x.DateComment.Year == condition.Year);
			}

			if (condition.Month > 0)
			{
				comments = comments.Where(x => x.DateComment.Month == condition.Month);
			}
			if (condition.PublishedOnly)
			{
				comments = comments.Where(x => x.Published);
			}
			if (condition.NotPublished)
			{
				comments = comments.Where(x => !x.Published);
			}
			return comments;
		}

			public async Task<bool> SetPublishedComment(int idComment, CancellationToken cancellationToken = default)
		{
			var comment = await _context.Set<Comment>().FindAsync(idComment);

			if (comment is null) return false;

			comment.Published = !comment.Published;

			await _context.SaveChangesAsync(cancellationToken);

			return true;
		}

        public Task<IList<CommentItem>> GetCommentsFromPostQuery(CommentQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

		public async Task<IPagedList<CommentItem>> GetPagedCommentsAsync(
		 IPagingParams pagingParams,
		 string name = null,
		 CancellationToken cancellationToken = default)
		{
			var CommentQuery = _context.Set<Comment>()
				.WhereIf(!string.IsNullOrWhiteSpace(name),
				x => x.Name.Contains(name))
					.Select(c => new CommentItem()
					{
						Id = c.Id,
						Name = c.Name,
						Published = c.Published,
						DateComment = c.DateComment,
						Description = c.Description,
						PostName = c.Post.Title,
						PostId = c.Post.Id
					});

			return await CommentQuery
				.ToPagedListAsync(pagingParams, cancellationToken);
		}
	}
}
