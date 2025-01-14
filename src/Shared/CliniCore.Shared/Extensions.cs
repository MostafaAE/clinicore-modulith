using CliniCore.Shared.Events;
using CliniCore.Shared.Exceptions;
using CliniCore.Shared.Messaging;
using CliniCore.Shared.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CliniCore.Shared;

public static class Extensions
{
    public static IServiceCollection AddSharedFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEvents();
        services.AddMessaging();
        services.AddSingleton<IClock, UtcClock>();

        services.AddSwaggerGen(swagger =>
        {
            var availabilityAssembly = Assembly.Load("CliniCore.Modules.Availability.Api");
            var availabilityXmlPath = Path.Combine(AppContext.BaseDirectory, $"{availabilityAssembly.GetName().Name}.xml");
            var bookingAssembly = Assembly.Load("CliniCore.Modules.Bookings.Api");
            var bookingsXmlPath = Path.Combine(AppContext.BaseDirectory, $"{bookingAssembly.GetName().Name}.xml");
            var appointmentsAssembly = Assembly.Load("CliniCore.Modules.Appointments.Shell");
            var appointmentsXmlPath = Path.Combine(AppContext.BaseDirectory, $"{appointmentsAssembly.GetName().Name}.xml");
            swagger.IncludeXmlComments(availabilityXmlPath);
            swagger.IncludeXmlComments(bookingsXmlPath);
            swagger.IncludeXmlComments(appointmentsXmlPath);
        });
        return services;
    }

    public static IApplicationBuilder UseSharedFramework(this IApplicationBuilder app)
    {
        app.UseErrorHandling();
        return app;
    }
}
