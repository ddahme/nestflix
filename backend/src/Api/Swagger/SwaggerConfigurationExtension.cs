using Microsoft.OpenApi.Models;

namespace Api.Swagger;

public static class SwaggerConfigurationExtension
{
    public static void ConfigureFullSwaggerConfig(this IServiceCollection col)
    {
        col.AddEndpointsApiExplorer();

        col.AddSwaggerGen(option =>
        {
            // Add OpenAPI document with valid version
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Nestflix API",
                Version = "v1",
                Description = "Nestflix API",
                Contact = new OpenApiContact
                {
                    Name = "Nestflix",
                }
            });
        });
    }
}