using CliniCore.Modules.Availability.Business;
using CliniCore.Modules.Availability.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Availability.Api;

public static class Extensions
{
    public static IServiceCollection AddAvailabiliyModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessLayer();
        services.AddDataLayer(configuration);
        return services;
    }

    public static IApplicationBuilder UseAvailabiliyModule(this IApplicationBuilder app)
    {
        return app;
    }
}
