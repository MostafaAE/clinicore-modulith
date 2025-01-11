using CliniCore.Modules.Confirmations.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Confirmations.Api;

public static class Extensions
{
    public static IServiceCollection AddConfirmationsModule(this IServiceCollection services)
    {
        services.AddScoped<IConfirmationSender, ConfirmationSender>();
        return services;
    }

    public static IApplicationBuilder UseConfirmationsModule(this IApplicationBuilder app)
    {
        return app;
    }
}
