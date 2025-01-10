using CliniCore.Shared.Events;
using CliniCore.Shared.Exceptions;
using CliniCore.Shared.Messaging;
using CliniCore.Shared.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Shared;

public static class Extensions
{
    public static IServiceCollection AddSharedFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEvents();
        services.AddMessaging();
        services.AddSingleton<IClock, UtcClock>();
        return services;
    }

    public static IApplicationBuilder UseSharedFramework(this IApplicationBuilder app)
    {
        app.UseErrorHandling();
        return app;
    }
}
