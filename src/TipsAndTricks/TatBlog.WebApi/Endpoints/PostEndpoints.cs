using FluentValidation;
using Mapster;
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
using TatBlog.WebApi.Models.PostsModel;

namespace TatBlog.WebApi.Endpoints
{
	public static class PostEndpoints
	{
		public static WebApplication MapPostEndpoints(
		this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/posts");

			routerGroupBuilder.MapGet("/", GetPosts)
				.WithName("GetPosts")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routerGroupBuilder.MapGet("/featured/{limit:int}", GetFeaturedPosts)
				.WithName("GetFeaturedPosts")
				.Produces<ApiResponse<IList<PostDto>>>();

			routerGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
				.WithName("GetRandomPosts")
				.Produces<ApiResponse<IList<Post>>>();

			routerGroupBuilder.MapGet("/archives/{limit:int}", GetArchivesPosts)
				.WithName("GetArchivesPosts")
				.Produces<ApiResponse<IList<DatePost>>>();

			routerGroupBuilder.MapGet("/{id:int}", GetPostsDetails)
				.WithName("GetPostById")
				.Produces<ApiResponse<PostDto>>();

			routerGroupBuilder.MapGet("/{id:int}/comments", GetCommentByPostId)
				.WithName("GetCommentByPostId")
				.Produces<ApiResponse<IList<CommentItem>>>();

			routerGroupBuilder.MapGet("/byslug/{slug}", GetPostsDetailsBySlug)
				.WithName("GetPostsDetailsBySlug")
				.Produces<ApiResponse<PostDto>>();

			routerGroupBuilder.MapPost(
				"/",
				AddPost)
				.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
				.WithName("AddNewPost")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<AuthorItem>>();

			

			routerGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
				.WithName("SetPostPicture")
				//.RequireAuthorization()
				.Accepts<IFormFile>("multipart/form-data")
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routerGroupBuilder.MapPut(
				"/{id:int}",
				UpdatePost)
				.WithName("UpdatePost")
				//.RequireAuthorization()
				.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routerGroupBuilder.MapDelete(
				"/{id:int}",
				DeletePost)
				.WithName("DeletePost")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetPosts(
			[AsParameters] PostFilterModel model,
			IBlogRepository blogRepository,
			IMapper mapper
			)
		{
			var postQuery = mapper.Map<PostQuery>(model);

			var postsList = await blogRepository.GetPagedPostAsync(
				model,
				posts => posts.ProjectToType<PostDto>(),
				postQuery);

			var paginationResult =
				new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetPostsDetails(
			int id,
			IMapper mapper,
			IBlogRepository blogRepository)
		{
			var post = await blogRepository
				.GetPostByIdAsync(id, true);
			return post == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
		}

		private static async Task<IResult> GetPostsDetailsBySlug(
			string slug,
			IMapper mapper,
			IBlogRepository blogRepository)
		{
			var post = await blogRepository
				.GetPostBySlugAsync(slug, true);
			return post == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có định danh {slug}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
		}
		private static async Task<IResult> GetFeaturedPosts(
			int limit,
			IBlogRepository blogRepository)
		{
			var posts = await blogRepository
				.GetFeaturePostMapperAysnc(limit, posts => posts.ProjectToType<PostDto>());
			return Results.Ok(ApiResponse.Success(posts));

		}

		private static async Task<IResult> GetRandomPosts(
			int limit,
			IBlogRepository blogRepository)
		{
			var posts = await blogRepository
				.GetPostsRandomAsync(limit);
			return Results.Ok(ApiResponse.Success(posts));

		}

		private static async Task<IResult> GetArchivesPosts(
			int limit,
			IBlogRepository blogRepository)
		{
			var posts = await blogRepository
				.GetPostCountByMonthArchives(limit);
			return Results.Ok(ApiResponse.Success(posts));

		}


		//private static async Task<IResult> GetPostsByAuthorId(
		//	int id,
		//	[AsParameters] PagingModel pagingModel,
		//	IBlogRepository blogRepository)
		//{
		//	var postQuery = new PostQuery()
		//	{
		//		AuthorId = id,
		//		PublishedOnly = true,
		//	};

		//	var postsList = await blogRepository.GetPagedListPostFromQueryableAsync(
		//		pagingModel,
		//		posts => posts.ProjectToType<PostDto>(),
		//		 postQuery);

		//	var pagingationResult = new PaginationResult<PostDto>(postsList);

		//	return Results.Ok(ApiResponse.Success(pagingationResult));
		//}

		private static async Task<IResult> GetCommentByPostId(
			int id,
			ICommentRepository commentRepository)
		{
			var comment = commentRepository.GetCommentsFromPostIDAsync(id);

			return Results.Ok(ApiResponse.Success(comment));
		}
		private static async Task<IResult> AddPost(
			PostEditModel model,
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			if (await blogRepository
				.IsPostSlugExitedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}
			var category = await blogRepository.GetCategoryFromIDAsync(model.CategoryId);
			var author = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

			if (category == null || author == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,"Nhập sai Id Chủ đề hoặc Tác giả"));

			}
			var post = mapper.Map<Post>(model);
			post.PostedDate = DateTime.Now;
			await blogRepository.AddUpdatePostAsync(post,model.GetSelectedTags());

			return Results.Ok(ApiResponse.Success(
				mapper.Map<AuthorItem>(post), HttpStatusCode.Created));
		}

		private static async Task<IResult> SetPostPicture(
			int id, IFormFile imageFile,
			IBlogRepository blogRepository,
			IMediaManager mediaManager)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(),
				imageFile.FileName,
				imageFile.ContentType);
			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
			}

			await blogRepository.SetPostImageUrlAsync(id, imageUrl);

			return Results.Ok(ApiResponse.Success(imageUrl));
		}

		private static async Task<IResult> UpdatePost(
			int id, PostEditModel model,
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository,
			IValidator<PostEditModel> validator,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
			}
			var post = await blogRepository.GetPostByIdAsync(id, false);
			if (post == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy bài viết"));
			}
				
			if (await blogRepository.IsPostSlugExitedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}
			var category = await blogRepository.GetCategoryFromIDAsync(model.CategoryId);
			var author = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

			if (category == null || author == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, "Nhập sai Id Chủ đề hoặc Tác giả"));

			}
			mapper.Map(model, post);
			post.Category = null;
			post.ModifiedDate = DateTime.Now;
			post.Id = id;
			
			return await blogRepository.AddUpdatePostAsync(post,model.GetSelectedTags())
				? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));

		}

		private static async Task<IResult> DeletePost(
			int id,
			IBlogRepository blogRepository)
		{
			return await blogRepository.DeletePostAsync(id)
				? Results.Ok(ApiResponse.Success("Post is Deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find post"));
		}
	}
}
