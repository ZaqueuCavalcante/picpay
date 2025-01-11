using Scalar.AspNetCore;

namespace PicPay.Api.Middlewares;

public static class HttpMiddlewares
{
    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();

            options.MapOpenApi();

            options.MapScalarApiReference(options =>
            {
                options.WithModels(false);
                options.WithDownloadButton(false);
                options.WithTitle("PicPay API Docs");
                options.WithEndpointPrefix("/docs/{documentName}");
                options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
                options
                    .WithPreferredScheme("Bearer")
                    .WithHttpBearerAuthentication(bearer =>
                    {
                        bearer.Token = "your.bearer.token";
                    })
                    .WithHttpBasicAuthentication(basic => {});
            });
        });
    }
}
