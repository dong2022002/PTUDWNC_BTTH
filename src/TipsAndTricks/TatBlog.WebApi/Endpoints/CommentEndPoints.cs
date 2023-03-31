using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.CommentsModel;

namespace TatBlog.WebApi.Endpoints
{
	public static class CommentEndPoints
	{
		public static WebApplication MapCommentsEndpoints(
			this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/comments");

		routerGroupBuilder.MapGet("/", GetComments)
				.WithName("GetComments")
				.Produces<ApiResponse<PaginationResult<CommentItem>>>();

			routerGroupBuilder.MapGet("/{id:int}", GetCommentsDetails)
				.WithName("GetCommentsDetails")
				.Produces<ApiResponse<CommentItem>>();

			routerGroupBuilder.MapGet("/{id:int}/Post", GetCommetsByPostId)
				.WithName("GetCommetsByPostId")
				.Produces<ApiResponse<CommentItem>>();

			routerGroupBuilder.MapPost(
				"/",
				AddComment)
				.AddEndpointFilter<ValidatorFilter<CommentAddModel>>()
				.WithName("AddNewComment")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<CommentItem>>();


			routerGroupBuilder.MapPost("/{id:int}/Published", SetPublishedComment)
				.WithName("SetPublishedComment")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routerGroupBuilder.MapPut(
				"/{id:int}",
				UpdateComment)
				.WithName("UpdateComment")
				//.RequireAuthorization()
				.AddEndpointFilter<ValidatorFilter<CommentUpdateModel>>()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routerGroupBuilder.MapDelete(
				"/{id:int}",
				DeleteComment)
				.WithName("DeleteComment")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetComments(
			[AsParameters] CommentFilterModel model,
			ICommentRepository commentRepository
			)
		{
			var commentList = await commentRepository
				.GetPagedCommentsAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<CommentItem>(commentList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetCommentsDetails(
			int id,
			IMapper mapper,
			ICommentRepository commentRepository)
		{
			var comment = await commentRepository
				.GetCommentsByIDAsync(id);
			return comment == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bình luận có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<CommentItem>(comment)));
		}

		private static async Task<IResult> GetCommetsByPostId(
			int id,
			ICommentRepository commentRepository)
		{
			var commentList = await commentRepository
				.GetCommentsFromPostIDAsync(id);

			return Results.Ok(ApiResponse.Success(commentList));
		}




		private static async Task<IResult> AddComment(
			CommentAddModel model,
			ICommentRepository commentRepository,
			IMapper mapper)
		{
			var comment = mapper.Map<Comment>(model);
			comment.Published = false;
			comment.DateComment = DateTime.Now;

			var rs = await commentRepository.AddUpdateCommentAsync(comment);

			return rs ? Results.Ok(ApiResponse.Success(
				mapper.Map<AuthorItem>(comment), HttpStatusCode.Created))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Thêm thất bại"));
		}

		private static async Task<IResult> SetPublishedComment(
			int id,
			ICommentRepository commentRepository,
			IMapper mapper)
		{
			return await commentRepository.SetPublishedComment(id) 
				? Results.Ok(ApiResponse.Success(
				"Cập nhật trạng thái bình luận thành công", HttpStatusCode.Created))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Cập nhật thất bại"));
		}



		private static async Task<IResult> UpdateComment(
			int id, CommentUpdateModel model,
			ICommentRepository commentRepository,
			IValidator<CommentUpdateModel> validator,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
			}
			var comment = await commentRepository.GetCommentsByIDAsync(id);
			if (comment ==null)
			{
			return	Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find comment"));
			}
			comment.Description = model.Description;
			await commentRepository.AddUpdateCommentAsync(comment);

			return Results.Ok(ApiResponse.Success("Comment is updated", HttpStatusCode.NoContent));
			

		}

		private static async Task<IResult> DeleteComment(
			int id,
			ICommentRepository commentRepository)
		{
			return await commentRepository.DeleteCommentAsync(id)
				? Results.Ok(ApiResponse.Success("Comment is Deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find Comment"));
		}
	}
}
