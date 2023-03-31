﻿using FluentValidation;
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

			//	routerGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
			//		.WithName("SetAuthorPicture")
			//		//.RequireAuthorization()
			//		.Accepts<IFormFile>("multipart/form-data")
			//		.Produces(401)
			//		.Produces<ApiResponse<string>>();

			//	routerGroupBuilder.MapPut(
			//		"/{id:int}",
			//		UpdateAuthor)
			//		.WithName("UpdateAuthor")
			//		//.RequireAuthorization()
			//		.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
			//		.Produces(401)
			//		.Produces<ApiResponse<string>>();

			//	routerGroupBuilder.MapDelete(
			//		"/{id:int}",
			//		DeleteAuthor)
			//		.WithName("DeleteAuthor")
			//		//.RequireAuthorization()
			//		.Produces(401)
			//		.Produces<ApiResponse<string>>();

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

		//private static async Task<IResult> SetAuthorPicture(
		//	int id, IFormFile imageFile,
		//	IAuthorRepository authorRepository,
		//	IMediaManager mediaManager)
		//{
		//	var imageUrl = await mediaManager.SaveFileAsync(
		//		imageFile.OpenReadStream(),
		//		imageFile.FileName, imageFile.ContentType);
		//	if (string.IsNullOrWhiteSpace(imageUrl))
		//	{
		//		return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
		//	}

		//	await authorRepository.SetImageUrlAsync(id, imageUrl);

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
		//	if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug))
		//	{
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