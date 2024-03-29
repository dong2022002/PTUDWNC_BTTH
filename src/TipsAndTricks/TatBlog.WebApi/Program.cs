
using TatBlog.WebApi.Endpoints;
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.validations;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpentApi()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}

var app = builder.Build();
{
	app.SetupRequestPipeline();

	app.MapAuthorEndpoints();
	app.MapCategoryEndpoints();
	app.MapPostEndpoints();
	app.MapTagEndpoints();
	app.MapSubscriberEndpoints();
	app.MapCommentsEndpoints();
	app.MapContactEndpoints();
	app.MapDashboardEndpoints();

	app.Run();
}
