// Infrastructure/DependencyInjection.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Documentation;
using Microsoft.AspNetCore.Builder;
using Infrastructure.Cors;


namespace Infrastructure;

public static class AddInfrastructureExtension
{
    /// <summary>
    /// Registra los componentes necesario de la infraestructura
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddOpenApiDocumentation(config)
            .AddCorsPolicy(config);

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>

        builder
            .UseOpenApiDocumentation(config)
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseHttpsRedirection()
            .UseCors();

}
