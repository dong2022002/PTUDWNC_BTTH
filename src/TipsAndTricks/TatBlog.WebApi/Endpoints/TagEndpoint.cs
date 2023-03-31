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
using TatBlog.WebApi.Models.TagsModel;

namespace TatBlog.WebApi.Endpoints
{
	public static class TagEndpoint
	{
		public static WebApplication MapTagEndpoints(
		this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/tags");

			routerGroupBuilder.MapGet("/", GetTags)
				.WithName("GetTags")
				.Produces<ApiResponse<PaginationResult<TagItem>>>();




			routerGroupBuilder.MapGet("/{id:int}", GetTagsDetails)
				.WithName("GetTagById")
				.Produces<ApiResponse<TagItem>>();

			routerGroupBuilder.MapPost(
				"/",
				AddTag)
				.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
				.WithName("AddNewTag")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<TagItem>>();

			routerGroupBuilder.MapGet(
				"/{slug:regex(^[a-z0-9_-]+$)}/posts",
				GetPostsByTagSlug)
				.WithName("GetPostsByTagSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();


			routerGroupBuilder.MapPut(
				"/{id:int}",
				UpdateTag)
				.WithName("UpdateTag")
				//.RequireAuthorization()
				.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routerGroupBuilder.MapDelete(
				"/{id:int}",
				DeleteTag)
				.WithName("DeleteTag")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetTags(
			[AsParameters] TagFilterModel model,
			IBlogRepository blogRepository
			)
		{
			var tagsList = await blogRepository
				.GetPagedTagsAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<TagItem>(tagsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetTagsDetails(
			int id,
			IMapper mapper,
			IBlogRepository blogRepository)
		{
			var tag = await blogRepository
				.GetTagFromIdAsync(id);
			return tag == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
		}


		private static async Task<IResult> GetPostsByTagId(
			int id,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				TagId = id,
				PublishedOnly = true,
			};

			var postsList = await blogRepository.GetPagedPostAsync(
				pagingModel,
				posts => posts.ProjectToType<PostDto>(),
				 postQuery);

			var pagingationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(pagingationResult));
		}

		private static async Task<IResult> GetPostsByTagSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				TagSlug = slug,
				PublishedOnly = true,
			};

			var postsList = await blogRepository.GetPagedPostAsync(
				pagingModel,
				posts => posts.ProjectToType<PostDto>(),
				 postQuery);

			var pagingationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(pagingationResult));
		}
		private static async Task<IResult> AddTag(
			TagEditModel model,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			if (await blogRepository
				.IsTagSlugExitedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var tag = mapper.Map<Tag>(model);
			await blogRepository.AddOrUpdateTagAsync(tag);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<TagItem>(tag), HttpStatusCode.Created));
		}

		private static async Task<IResult> UpdateTag(
			int id, TagEditModel model,
			IBlogRepository blogRepository,
			IValidator<TagEditModel> validator,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
			}
			if (await blogRepository.IsTagSlugExitedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var tag = mapper.Map<Tag>(model);

			tag.Id = id;

			return await blogRepository.AddOrUpdateTagAsync(tag)
				? Results.Ok(ApiResponse.Success("Tag is updated", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find tag"));

		}

		private static async Task<IResult> DeleteTag(
			int id,
			IBlogRepository blogRepository)
		{
			return await blogRepository.delTagAsync(id)
				? Results.Ok(ApiResponse.Success("Tag is Deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find tag"));
		}
	}
}
