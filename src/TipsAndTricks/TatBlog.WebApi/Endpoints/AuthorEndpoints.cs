using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class AuthorEndpoints
	{

		public static WebApplication MapAuthorEndpoints(
			this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/authors");

			routerGroupBuilder.MapGet("/", GetAuthors)
				.WithName("GetAuthors")
				.Produces<PaginationResult<AuthorItem>>();


			routerGroupBuilder.MapGet("/{id:int}", GetAuthorsDetails)
				.WithName("GetAuthorById")
				.Produces<AuthorItem>()
				.Produces(404);

			routerGroupBuilder.MapPost(
				"/",
				AddAuthor)
				.WithName("AddNewAuthor")
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.Produces(201)
				.Produces(400)
				.Produces(409);

			routerGroupBuilder.MapPost(
				"/{slug:regex(^[a-z0-9_-]+$)}/posts",
				GetPostsByAuthorSlug)
				.WithName("GetPostsByAuthorSlug")
				.Produces<PaginationResult<PostDto>>();

			routerGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
				.WithName("SetAuthorPicture")
				.Accepts<IFormFile>("multipart/form-data")
				.Produces<string>()
				.Produces(400);

			routerGroupBuilder.MapPut(
				"/{id:int}",
				UpdateAuthor)
				.WithName("UpdateAuthor")
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.Produces(204)
				.Produces(400)
				.Produces(409);

			routerGroupBuilder.MapDelete(
				"/{id:int}",
				DeleteAuthor)
				.WithName("DeleteAuthor")
				.Produces(204)
				.Produces(404);

			return app;
		}

		private static async Task<IResult> GetAuthors(
			[AsParameters] AuthorFilterModel model,
			IAuthorRepository authorRepository)
		{
			var authorsList = await authorRepository
				.GetPagedAuthorsAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<AuthorItem>(authorsList);

			return Results.Ok(paginationResult);
		}

		private static async Task<IResult> GetAuthorsDetails(
			int id,
			IMapper mapper,
			IAuthorRepository authorRepository)
		{
			var author = await authorRepository
				.GetCachedAuthorByIdAsync(id);
			return author == null
				? Results.NotFound($"Không tìm thấy tác giã có mã số {id}")
				: Results.Ok(mapper.Map<AuthorItem>(author));
		}

		private static async Task<IResult> GetPostsByAuthorId(
			int id,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				AuthorId = id,
				PublishedOnly = true,
			};

			var postsList = await blogRepository.GetPagedListPostFromQueryableAsync(
				pagingModel,
				posts => posts.ProjectToType<PostDto>(),
				 postQuery);

			var pagingationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(pagingationResult);
		}

		private static async Task<IResult> GetPostsByAuthorSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				AuthorSlug = slug,
				PublishedOnly = true,
			};

			var postsList = await blogRepository.GetPagedListPostFromQueryableAsync(
				pagingModel,
				posts => posts.ProjectToType<PostDto>(),
				 postQuery);

			var pagingationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(pagingationResult);
		}
		private static async Task<IResult> AddAuthor(
			AuthorEditModel model,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
            if (await authorRepository
				.IsAuthorSlugExistedAsync(0,model.UrlSlug))
            {
				return Results.Conflict(
					$"Slug '{model.UrlSlug}' đã được sử dụng");
            }

			var author = mapper.Map<Author>(model);
			await authorRepository.AddOrUpdateAsync(author);

			return Results.CreatedAtRoute(
				"GetAuthorById", new { author.Id },
				mapper.Map<AuthorItem>(author));
        }

		private static async Task<IResult> SetAuthorPicture(
			int id, IFormFile imageFile,
			IAuthorRepository authorRepository,
			IMediaManager mediaManager)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(),
				imageFile.FileName, imageFile.ContentType);
			if(string.IsNullOrWhiteSpace(imageUrl))
			{
				return Results.BadRequest("Không lưu được tập tin");
			}

			await authorRepository.SetImageUrlAsync(id,imageUrl);

			return Results.Ok(imageUrl);
		}

		private static async Task<IResult> UpdateAuthor(
			int id, AuthorEditModel model,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			if (await authorRepository
			   .IsAuthorSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Conflict(
					$"Slug '{model.UrlSlug}' đã được sử dụng");
			}
			var author = mapper.Map<Author>(model);
			author.Id = id;

			return await authorRepository.AddOrUpdateAsync(author)
				? Results.NoContent() : Results.NotFound();
		}

		private static async Task<IResult> DeleteAuthor(
			int id,
			IAuthorRepository authorRepository)
		{
			return await authorRepository.DeleteAuthorAsync(id)
				? Results.NoContent() : Results.NotFound($"Could not find author with id = {id}");
		}
	}
}
