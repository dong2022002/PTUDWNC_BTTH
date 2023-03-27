
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpentApi()
		.ConfigureMapster();
}

var app = builder.Build();
{
	app.SetupRequestPipeline();

	app.Run();
}
