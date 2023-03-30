using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog.Web;
using System.Net;
using System.Net.Mail;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Extensions;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;

namespace TatBlog.WebApp.Extensions
{
	public static class WebApplicationExtensions
	{

		public static WebApplicationBuilder ConfigureCors(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("TatBlogApp", policyBuilder =>
						policyBuilder
						   .AllowAnyOrigin()
						   .AllowAnyHeader()
						   .AllowAnyMethod());
			
			});
	

			return builder;
		}

		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddMemoryCache();

			builder.Services.AddDbContext<BlogDbContext>(options =>
					options.UseSqlServer(
								builder.Configuration
									.GetConnectionString("DefaultConnection")));
			builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
			builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
			return builder;

		}

		public static WebApplication SetupRequestPipeline(
			this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseCors("TatBlogApp");
			return app;
		}
		public static WebApplicationBuilder ConfigureSwaggerOpentApi(
					this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			return builder;
		}
		public static WebApplicationBuilder ConfigureNLog(
			this WebApplicationBuilder builder)
		{
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();

			return builder;
		}
	}
}
