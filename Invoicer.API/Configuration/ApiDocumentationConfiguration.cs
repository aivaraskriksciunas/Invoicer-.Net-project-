namespace Invoicer.Api.Configuration;

using System.Reflection;
using Microsoft.OpenApi.Models;

public static class ApiDocumentationConfiguration
{
    public static void AddApiDocumentation( this IServiceCollection services )
    {
        services.AddSwaggerGen( options =>
        {
            options.SwaggerDoc( "v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Invoicer API",
                Description = "Personal and business invoice management tool.",
                Contact = new OpenApiContact
                {
                    Name = "Aivaras Kriksciunas",
                },
            } );

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments( Path.Combine( AppContext.BaseDirectory, xmlFilename ) );
        } );
    }

    public static void UseApiDocumentationUI( this WebApplication app )
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
