using Hangfire;
using PicPay.Worker.Tasks;
using PicPay.Worker.Events;
using PicPay.Worker.Filters;
using PicPay.Worker.Configs;
using Hangfire.MemoryStorage;
using PicPay.Worker.Extensions;

namespace PicPay.Worker;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServicesConfigs();
        services.AddHandlersConfigs();

        services.AddDapperConfigs();

        services.AddHostedService<DomainEventsProcessorDbListener>();
        services.AddHostedService<PicPayTasksProcessorDbListener>();

        services.AddHangfire(x =>
        {
            x.UseMemoryStorage();
            x.UseRecommendedSerializerSettings();
            x.UseSimpleAssemblyNameTypeSerializer();
        });

        services.AddHangfireServer(x =>
        {
            x.ServerName = "Worker";
            x.SchedulePollingInterval = TimeSpan.FromSeconds(60);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        BackgroundJob.Enqueue<DomainEventsProcessor>(x => x.Run());
        BackgroundJob.Enqueue<PicPayTasksProcessor>(x => x.Run());

        app.UseRouting();
        app.UseStaticFiles();

        app.UseEndpoints(x =>
        {
            x.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));
        });

        app.UseHangfireDashboard(
            pathMatch: "",
            options: new DashboardOptions
            {
                FaviconPath = "/favicon.ico",
                Authorization = [ new HangfireAuthFilter(configuration.Hangfire().User, configuration.Hangfire().Password) ]
            }
        );
    }
}
