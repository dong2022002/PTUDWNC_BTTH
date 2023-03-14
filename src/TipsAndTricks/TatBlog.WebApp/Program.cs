

using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.validations;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}

var app = builder.Build();
{
	app.UseRequestPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}

app.Run();

