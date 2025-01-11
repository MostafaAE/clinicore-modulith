using CliniCore.Modules.Availability.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Availability.Data;

public static class Extensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISlotRepository, SlotRepository>();

        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<AvailabilityDbContext>(options => {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}
