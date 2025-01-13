using CliniCore.Modules.Appointments.Core;
using CliniCore.Modules.Appointments.Core.InputPorts;
using CliniCore.Modules.Appointments.Core.OutputPorts;
using CliniCore.Modules.Appointments.Core.Services;
using CliniCore.Modules.Appointments.Shell.DAL;
using CliniCore.Modules.Appointments.Shell.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Appointments.Shell;

public static class Extensions
{
    public static IServiceCollection AddAppointmentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAppointmentsService, AppointmentService>();

        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<AppointmentsDbContext>(options => {
            options.UseSqlite(connectionString);
        });

        services.AddCoreLayer();
        return services;
    }

    public static IApplicationBuilder UseAppointmentsModule(this IApplicationBuilder app)
    {
        return app;
    }
}
