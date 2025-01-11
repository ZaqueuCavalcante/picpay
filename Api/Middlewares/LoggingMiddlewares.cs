using Serilog;

namespace PicPay.Api.Middlewares;

public static class LoggingMiddlewares
{
    public static void UseLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}
