using CliniCore.Modules.Availability.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Tests.Shared;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registrations
            RemoveDbContextRegistrations(services);

            // Add all DbContexts with an in-memory database
            AddInMemoryDbContexts(services);
        });
    }

    private void RemoveDbContextRegistrations(IServiceCollection services)
    {
        var dbContextDescriptors = services
            .Where(service => service.ServiceType.IsGenericType &&
                              service.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
            .ToList();

        foreach (var descriptor in dbContextDescriptors)
        {
            services.Remove(descriptor);
        }
    }

    private void AddInMemoryDbContexts(IServiceCollection services)
    {
        services.AddDbContext<AvailabilityDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName);
        });
    }
}
