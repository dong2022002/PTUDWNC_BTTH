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


			routerGroupBuilder.MapPut(
				"/{id:int}",
				UpdateCategory)
				.WithName("UpdateCategory")
				//.RequireAuthorization()
				.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routerGroupBuilder.MapDelete(
				"/{id:int}",
				DeleteCategory)
				.WithName("DeleteCategory")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<string>>();

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

			var postsList = await blogRepository.GetPagedPostAsync(
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
		  .IsCategorySlugExistedAsync(0,model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var category = mapper.Map<Category>(model);
			await blogRepository.AddUpdateCategoryAsync(category);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
		}

		
		private static async Task<IResult> UpdateCategory(
			int id, CategoryEditModel model,
			IBlogRepository blogRepository,
			IValidator<CategoryEditModel> validator,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
			}
			if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}
			var category = mapper.Map<Category>(model);
			category.Id = id;

			return await blogRepository.AddOrUpdateCategoryAsync(category)
				? Results.Ok(ApiResponse.Success("Category is updated", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find category"));

		}

		private static async Task<IResult> DeleteCategory(
			int id,
			IBlogRepository blogRepository)
		{
			return await blogRepository.DeleteCategoryAsync(id)
				? Results.Ok(ApiResponse.Success("Category is Deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find category"));
		}
	}
}
