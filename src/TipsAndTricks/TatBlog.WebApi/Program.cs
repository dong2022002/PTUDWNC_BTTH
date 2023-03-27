
using TatBlog.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpentApi();
}

var app = builder.Build();
{
	app.SetupRequestPipeline();

	app.Run();
}
