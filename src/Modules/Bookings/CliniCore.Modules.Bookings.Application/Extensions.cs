using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Bookings.Application;

public static class Extensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<GetAvailableBookingsHandler>();

        return services;
    }
}
