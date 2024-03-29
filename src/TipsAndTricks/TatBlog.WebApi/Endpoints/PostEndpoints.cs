﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Extensions;
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

			routerGroupBuilder.MapGet("get-filter", GetFilter)
				.WithName("GetFilter")
				.Produces<ApiResponse<PostDataFilter>>();

			routerGroupBuilder.MapGet("/{id:int}/comments", GetCommentByPostId)
				.WithName("GetCommentByPostId")
				.Produces<ApiResponse<IList<CommentItem>>>();

			routerGroupBuilder.MapGet("/byslug/{slug}", GetPostsDetailsBySlug)
				.WithName("GetPostsDetailsBySlug")
				.Produces<ApiResponse<PostDto>>();

			routerGroupBuilder.MapPost(
				"/",
				AddPost)
				.Accepts<PostEditModel>("multipart/form-data")
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
				"/{id:int}/changePublished",
				ChangePublished)
				.WithName("ChangePublished")
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

		private static async Task<IResult> ChangePublished(
			int id,
			IBlogRepository blogRepository)
		{
			await blogRepository
				.SetPublishedPostAsync(id);
			return Results.Ok(ApiResponse.Success("Thay đổi thành công"));

		}


		private static async Task<IResult> GetCommentByPostId(
			int id,
			ICommentRepository commentRepository)
		{
			var comment =await commentRepository.GetCommentsFromPostIDAsync(id);

			return Results.Ok(ApiResponse.Success(comment));
		}
		private static async Task<IResult> GetFilter(
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository)
		{
			PostDataFilter postDataFilter = new PostDataFilter();
			var authors = await authorRepository.GetAuthorsAsync();
			var categories = await blogRepository.GetCategoriesAsync();
			postDataFilter.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});
			postDataFilter.CategoryList = categories.Select(a => new SelectListItem()
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
			return Results.Ok(ApiResponse.Success(postDataFilter));
		}
		private static async Task<IResult> AddPost(
			HttpContext context,
			IMediaManager mediaManager,
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			var model = await PostEditModel.BindAsync(context);
			var slug = model.Title.GenerateSlug();

			if (await blogRepository
				.IsPostSlugExitedAsync(model.Id, slug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			
			var category = await blogRepository.GetCategoryFromIDAsync(model.CategoryId);
			var author = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

			if (category == null || author == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, "Nhập sai Id Chủ đề hoặc Tác giả"));

			}
			var post = model.Id > 0 ? await blogRepository.GetPostByIdAsync(model.Id, true)
				: null;
			if (post == null)
			{
				post = new Post()
				{
					PostedDate = DateTime.Now
				};
			}
			
			post.Title = model.Title;
			post.AuthorId = model.AuthorId;
			post.CategoryId = model.CategoryId;
			post.ShortDescription = model.ShortDescription;
			post.Description = model.Description;
			post.Meta = model.Meta;
			post.Published = model.Published;
			post.ModifiedDate = DateTime.Now;
			post.UrlSlug = model.Title.GenerateSlug();

            if (model.ImageFile?.Length > 0)
            {
				string hostname =
					$"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
				string uploadedPath = await mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
					post.ImageUrl = hostname + uploadedPath;
                }
            }
			await blogRepository.AddUpdatePostAsync(post, model.GetSelectedTags());

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
