
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class AddApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        
        // ¡Registra tu nuevo handler de propiedades!
        services.AddScoped<Application.Features.Properties.Queries.GetPropertyQueryHandler>();
        
        /*
        
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly);
        */
        return services;

    }
}
