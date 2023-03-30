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
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class CategoryEndpoints
	{
		
		public static WebApplication MapCategoryEndpoints(
			this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/categories");

			routerGroupBuilder.MapGet("/", GetCategories)
				.WithName("GetCategories")
				.Produces<ApiResponse<PaginationResult<CategoryItem>>>();

			//routerGroupBuilder.MapGet("/best/{limit:int}", GetBestAuthor)
			//	.WithName("GetBestAuthors")
			//	.Produces<ApiResponse<IList<AuthorItem>>>();


			routerGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
				.WithName("GetCategoryById")
				.Produces<ApiResponse<CategoryItem>>();

			routerGroupBuilder.MapPost(
				"/",
				AddCategory)
				.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
				.WithName("AddCategory")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<CategoryItem>>();

			routerGroupBuilder.MapGet(
				"/{slug:regex(^[a-z0-9_-]+$)}/posts",
				GetPostsByCategoriesSlug)
				.WithName("GetPostsByCategoriesSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			//routerGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
			//	.WithName("SetAuthorPicture")
			//	//.RequireAuthorization()
			//	.Accepts<IFormFile>("multipart/form-data")
			//	.Produces(401)
			//	.Produces<ApiResponse<string>>();

			//routerGroupBuilder.MapPut(
			//	"/{id:int}",
			//	UpdateAuthor)
			//	.WithName("UpdateAuthor")
			//	//.RequireAuthorization()
			//	.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
			//	.Produces(401)
			//	.Produces<ApiResponse<string>>();

			//routerGroupBuilder.MapDelete(
			//	"/{id:int}",
			//	DeleteAuthor)
			//	.WithName("DeleteAuthor")
			//	//.RequireAuthorization()
			//	.Produces(401)
			//	.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetCategories(
			[AsParameters] CategoryFilterModel model,
			IBlogRepository blogRepository
			)
		{
			var categories = await blogRepository
				.GetPagedCategoriesAsync(model,model.Name);

			var paginationResult =
				new PaginationResult<CategoryItem>(categories);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetCategoryDetails(
			int id,
			IMapper mapper,
			IBlogRepository blogRepository)
		{
			var category = await blogRepository
				.GetCategoryFromIDAsync(id);
			return category == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giã có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
		}
		//private static async Task<IResult> GetBestAuthor(
		//	int limit,
		//	IMapper mapper,
		//	IAuthorRepository authorRepository)
		//{
		//	var authors = await authorRepository
		//		.GetBestAuthorsAsync(limit);
		//	return Results.Ok(ApiResponse.Success(authors));

		//}

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

		private static async Task<IResult> GetPostsByCategoriesSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				CategorySlug = slug,
				PublishedOnly = true,
			};

			var postsList = await blogRepository.GetPagedListPostFromQueryableAsync(
				pagingModel,
				posts => posts.ProjectToType<PostDto>(),
				postQuery);

			var pagingationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(pagingationResult));
		}
		private static async Task<IResult> AddCategory(
			CategoryEditModel model,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			if (await blogRepository
		  .IsCategorySlugExistedAsync(model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var category = mapper.Map<Category>(model);
			await blogRepository.AddUpdateCategoryAsync(category);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
		}

		//private static async Task<IResult> SetAuthorPicture(
		//	int id, IFormFile imageFile,
		//	IAuthorRepository authorRepository,
		//	IMediaManager mediaManager)
		//{
		//	var imageUrl = await mediaManager.SaveFileAsync(
		//		imageFile.OpenReadStream(),
		//		imageFile.FileName, imageFile.ContentType);
		//	if(string.IsNullOrWhiteSpace(imageUrl))
		//	{
		//		return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
		//	}

		//	await authorRepository.SetImageUrlAsync(id,imageUrl);

		//	return Results.Ok(ApiResponse.Success(imageUrl));
		//}

		//private static async Task<IResult> UpdateAuthor(
		//	int id, AuthorEditModel model,
		//	IAuthorRepository authorRepository,
		//	IValidator<AuthorEditModel> validator,
		//	IMapper mapper)
		//{
		//	var validationResult = await validator.ValidateAsync(model);
		//	if (!validationResult.IsValid)
		//	{
		//		return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
		//	}
		//          if (await authorRepository.IsAuthorSlugExistedAsync(id,model.UrlSlug))
		//          {
		//		return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
		//	}
		//	var author = mapper.Map<Author>(model);
		//	author.Id = id;

		//	return await authorRepository.AddOrUpdateAsync(author)
		//		? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent))
		//		: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));

		//}

		//private static async Task<IResult> DeleteAuthor(
		//	int id,
		//	IAuthorRepository authorRepository)
		//{
		//	return await authorRepository.DeleteAuthorAsync(id)
		//		? Results.Ok(ApiResponse.Success("Author is Deleted", HttpStatusCode.NoContent))
		//		: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
		//}
	}
}
