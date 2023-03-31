
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class DashboardEndpoints
	{
		public static WebApplication MapDashboardEndpoints(
		this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/dashboards");

			routeGroupBuilder.MapGet("/", GetDashboardInformation)
				.WithName("GetDashboardInformation")
				.Produces<ApiResponse<IList<Statistical>>>();

			return app;
		}


		private static async Task<IResult> GetDashboardInformation(
			IBlogRepository blogRepo)
		{
			var rs = await blogRepo.GetStatistical();

			return Results.Ok(ApiResponse.Success(rs));
		}
	}
}
