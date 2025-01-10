using CliniCore.Modules.Appointments.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Appointments.Shell;

public static class Extensions
{
    public static IServiceCollection AddAppointmentsModule(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddCoreLayer();
        return services;
    }

    public static IApplicationBuilder UseAppointmentsModule(this IApplicationBuilder app)
    {
        return app;
    }
}
