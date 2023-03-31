using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.SubscribersModel;

namespace TatBlog.WebApi.Endpoints
{
	public static class SubscriberEndpoint
	{
		public static WebApplication MapSubscriberEndpoints(
			this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/subscriber");

			routerGroupBuilder.MapGet("/", GetSubscriber)
				.WithName("GetSubscriber")
				.Produces<ApiResponse<PaginationResult<Subscriber>>>();

			routerGroupBuilder.MapGet("/{id:int}", GetSubscribersDetails)
				.WithName("GetSubsciberById")
				.Produces<ApiResponse<Subscriber>>();

			routerGroupBuilder.MapPost(
				"/",
				Subscriber)
				.AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
				.WithName("Subscriber")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<Subscriber>>();

			routerGroupBuilder.MapPost(
				"/unsubcriber",
				Unsubscriber)
				.AddEndpointFilter<ValidatorFilter<UnSubscriberEditModel>>()
				.WithName("Unsubscriber")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<Subscriber>>();

			routerGroupBuilder.MapPost(
				"/blocksubcriber",
				BlockSubscriber)
				.WithName("BlockSubscriber")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<Subscriber>>();

			routerGroupBuilder.MapGet(
				"/{email:regex(^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{{2,4}}$)}",

				GetSubscribersDetailsByEmail)
				.WithName("GetSubsciberByEmail")
				.Produces<ApiResponse<Subscriber>>();

			routerGroupBuilder.MapDelete(
				"/{id:int}",
				DeleteSubscriber)
				.WithName("DeleteSubscriber")
				//.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetSubscriber(
			[AsParameters] SubscriberFilterModel model,
			ISubscriberRepository subscriberRepository
			)
		{
			var authorsList = await subscriberRepository
				.GetPagedSubcriberAsync(model, model.Email);

			var paginationResult =
				new PaginationResult<Subscriber>(authorsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetSubscribersDetails(
			int id,
			ISubscriberRepository subscriberRepository)
		{
			var subscriber = await subscriberRepository
				.GetSubscriberByIdAsync(id);
			return subscriber == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy người theo dõi có mã số {id}"))
				: Results.Ok(ApiResponse.Success(subscriber));
		}

		private static async Task<IResult> GetSubscribersDetailsByEmail(
			string email,
			ISubscriberRepository subscriberRepository)
		{
			var subscriber = await subscriberRepository
				.GetSubscriberByEmailAsync(email);
			return subscriber == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy người theo dõi có email {email}"))
				: Results.Ok(ApiResponse.Success(subscriber));
		}

		private static async Task<IResult> Subscriber(
			SubscriberEditModel model,
			ISubscriberRepository subscriberRepository)
		{
			Regex rx = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
			if (!rx.IsMatch(model.Email))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Nhập sai định dạng email"));
			}
			if (await subscriberRepository
				.IsEmailExitedAsync(0,model.Email))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Email '{model.Email}' đã được sử dụng"));
			}

			
			var subcriber= await subscriberRepository.SubscriberAsync(model.Email);

			return Results.Ok(ApiResponse.Success(
				subcriber, HttpStatusCode.Created));
		}


		private static async Task<IResult> Unsubscriber(
			UnSubscriberEditModel model,
			ISubscriberRepository subscriberRepository)
		{
			Regex rx = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
			if (!rx.IsMatch(model.Email))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Nhập sai định dạng email"));
			}
			if (!(await subscriberRepository
				.IsEmailExitedAsync(0, model.Email)))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"không tìm thấy email"));
			}


			await subscriberRepository.UnSubscriberAsync(model.Email,model.Reason,true);

			return Results.Ok(ApiResponse.Success(
				"Hủy theo dõi thành công", HttpStatusCode.Created));
		}

		private static async Task<IResult> BlockSubscriber(
			BlockSubscriber model,
			ISubscriberRepository subscriberRepository)
		{
			var IsExited = await subscriberRepository
				.GetSubscriberByIdAsync(model.Id) ?? null;
			if ((IsExited)==null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"không tìm thấy Người theo dõi"));
			}
			await subscriberRepository.BlockSubscriberAsync(model.Id, model.Notes, model.Reason);

			return Results.Ok(ApiResponse.Success(
				"Block thành công", HttpStatusCode.Created));
		}


		private static async Task<IResult> DeleteSubscriber(
			int id,
			ISubscriberRepository subscriberRepository)
		{
			return await subscriberRepository.DeleteSubscriberAsync(id)
				? Results.Ok(ApiResponse.Success("Subscriber is Deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find Subscriber"));
		}
	}
}
