using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PhotoSiTest.API.Extensions.Swagger;

public static class SwaggerConfigExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoSì Test API", Version = "v1" });
            options.DocInclusionPredicate((_, _) => true);
            options.UseInlineDefinitionsForEnums();
        });
    }


    public static void ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => {
            options.DisplayRequestDuration();
            options.ShowCommonExtensions();
            options.DocExpansion(DocExpansion.List);
            options.ShowExtensions();
            options.ConfigObject.AdditionalItems.Add("syntaxHighlight", true);
            options.EnableFilter();
            options.EnableTryItOutByDefault();
        });
    }
}
