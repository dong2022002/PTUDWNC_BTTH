using NLog;
using NLog.Web;
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.validations;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);
    {
        builder
            .ConfigureMvc()
            .ConfigureNLog()
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

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

