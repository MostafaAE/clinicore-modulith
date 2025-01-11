using CliniCore.Modules.Bookings.Application;
using CliniCore.Modules.Bookings.Domain;
using CliniCore.Modules.Bookings.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Bookings.Api;

public static class Extensions
{
    public static IServiceCollection AddBookingsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationLayer();
        services.AddDomainLayer();
        services.AddInfrastructureLayer(configuration);
        return services;
    }

    public static IApplicationBuilder UseBookingsModule(this IApplicationBuilder app)
    {
        return app;
    }
}
