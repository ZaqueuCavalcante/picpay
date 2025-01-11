using System.Reflection;
using PicPay.Api.Filters;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace PicPay.Api.Configs;

public static class DocsConfigs
{
    public static void AddDocsConfigs(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PicPay API Docs",
                Description = ReadResource("api-intro.md"),
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-logo", new OpenApiObject
                    {
                        { "url", new OpenApiString("/picpay-logo.png") },
                    }}
                },
            });

            options.EnableAnnotations();

            options.TagActionsBy(api =>
            {
                var group = api.RelativePath.Split("/")[0];
                if (group == "customer") return ["🙍🏻‍♂️ Cliente"];
                if (group == "merchant") return ["🛒 Lojista"];
                if (group == "adm") return ["🛡️ Adm"];
                return ["🧱 Cross"];
            });
            options.DocInclusionPredicate((name, api) => true);

            options.OperationFilter<AuthOperationsFilter>();
            options.DocumentFilter<HttpMethodSorterDocumentFilter>();

            options.ExampleFilters();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
            });

            options.DescribeAllParametersInCamelCase();

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Api.xml");
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            var xmlPath2 = Path.Combine(AppContext.BaseDirectory, "Shared.xml");
            options.IncludeXmlComments(xmlPath2, includeControllerXmlComments: true);
        });

        services.AddSwaggerExamplesFromAssemblyOf(typeof(Program));
    }

    private static string ReadResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
