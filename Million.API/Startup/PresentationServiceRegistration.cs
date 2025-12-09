using Microsoft.OpenApi.Models;

namespace Million.API.Startup;

public static class PresentationServiceRegistration
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureCors(services, configuration);
        
        ConfigureSwagger(services);
        
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase
            });

        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        return services;
    }

    #region CORS Configuration
    private static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("OpenCorsPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });

            options.AddPolicy("ProductionCorsPolicy", policy =>
            {
                var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                    ?? new[] { "https://yourdomain.com" };
                
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });
    }
    #endregion

    #region Swagger Configuration
    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Million Properties API",
                Version = "v1",
                Description = "API RESTful para gestión de propiedades inmobiliarias",
                Contact = new OpenApiContact
                {
                    Name = "Million Properties Team",
                    Email = "contact@million.com",
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            var xmlFile = "Million.Properties.API.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key necesaria para acceder a los endpoints. Formato: X-API-KEY: {tu_api_key}",
                In = ParameterLocation.Header,
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        Scheme = "ApiKeyScheme",
                        Name = "X-API-KEY",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
    #endregion
}
