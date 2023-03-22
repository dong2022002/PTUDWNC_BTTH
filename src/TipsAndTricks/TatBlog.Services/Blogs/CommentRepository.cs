using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public async Task<Comment> AddUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
		{
			if (comment.Id <= 0)
			{
				_context.Comments.Add(comment);

			}
			else
			{
				_context.Set<Comment>().Update(comment);

			}
			await _context.SaveChangesAsync(cancellationToken);
			return comment;
		}

		public async Task<Comment> DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
		{
			var comment = _context.Set<Comment>()
			  .Where(t => t.Id == id);
			if (comment == null) return null;
			var commentData = await comment.FirstOrDefaultAsync(cancellationToken);
			await comment.ExecuteDeleteAsync(cancellationToken);
			return commentData;
		}

		public async Task<IList<CommentItem>> GetCommentsFromPostIDAsync(int idPost, int number = -1, CancellationToken cancellationToken = default)
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
										});
			if (number < 0)
			{
				return await commentPostId.ToListAsync(cancellationToken);
			}
			else
			{
				return await commentPostId.Take(number).ToListAsync(cancellationToken);
			}
		
			
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
			});
			if (condition.Name != null)
			{
				comments = comments.Where(x => x.Name == condition.Name);
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

			return comment.Published;
		}

		
	}
}
