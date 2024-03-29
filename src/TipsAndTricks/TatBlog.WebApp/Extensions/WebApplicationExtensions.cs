﻿using Castle.Core.Smtp;
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
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extensions
{
	public static class WebApplicationExtensions
	{

		public static WebApplicationBuilder ConfigureMvc(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddControllersWithViews();
			builder.Services.AddResponseCompression();
			return builder;
		}

		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<BlogDbContext>(options =>
					options.UseSqlServer(
								builder.Configuration
									.GetConnectionString("DefaultConnection")));
			builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();
			builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
			builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			builder.Services.AddScoped<ICommentRepository, CommentRepository>();
			return builder;

		}

		public static WebApplication UseRequestPipeline(
		this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				
			}
			else
			{

				app.UseExceptionHandler("/Blog/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<UserActivityMiddleware>();

            app.UseRouting();
			return app;
		}

		public static IApplicationBuilder UseDataSeeder(
			this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				try
				{
					scope.ServiceProvider
						.GetRequiredService<IDataSeeder>()
						.Initialize();
				}
				catch (Exception ex)
				{
					scope.ServiceProvider
						.GetRequiredService<ILogger<Program>>()
						.LogError(ex, "Could not insert data into database");
				}
			
			}
			return app;
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
