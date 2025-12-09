using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
namespace Million.Properties.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddSingleton(assembly);
        services.AddAutoMapper(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));


        return services;
    }
}
