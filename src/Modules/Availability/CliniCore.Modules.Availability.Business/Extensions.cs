using CliniCore.Modules.Availability.Business.Mappers;
using CliniCore.Modules.Availability.Business.Services;
using CliniCore.Modules.Availability.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Availability.Business;

public static class Extensions
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddScoped<SlotsService>();
        services.AddScoped<ISlotsMapper, SlotsMapper>();
        services.AddScoped<IAvailabilityModuleApi, AvailabilityModuleApi>();

        return services;
    }
}
