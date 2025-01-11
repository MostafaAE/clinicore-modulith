using CliniCore.Modules.Bookings.Domain.Contracts;
using CliniCore.Modules.Bookings.Infrastructure.DAL;
using CliniCore.Modules.Bookings.Infrastructure.DAL.Repositories;
using CliniCore.Modules.Bookings.Infrastructure.EventPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Modules.Bookings.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IBookingPublisher, BookingPublisher>();
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<BookingDbContext>(options => {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}
