using Million.API.Startup;
using Million.Properties.API.Middlewares;
using Million.Properties.Application;
using Million.Properties.Application.Mappings;
using Million.Properties.Infrastructure.Persistence;
using Million.Properties.Infrastructure.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Million Properties API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Million Properties API";
    });
}

var corsPolicy = app.Environment.IsDevelopment() ? "OpenCorsPolicy" : "ProductionCorsPolicy";
app.UseCors(corsPolicy);

app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

using (var scope = app.Services.CreateScope())
{
    try
    {
        var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await initializer.InitializeAsync();
        app.Logger.LogInformation("Base de datos inicializada correctamente");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error al inicializar la base de datos");
        throw;
    }
}

app.Logger.LogInformation("Iniciando Million Properties API...");
app.Run();