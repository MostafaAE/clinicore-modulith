using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Tests.Shared.Utils;

public static class TestUtils
{

    /// <summary>
    /// Clears and recreates the database for all DbContext types.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public static async Task ClearDatabaseAsync(CustomWebApplicationFactory factory)
    {
        // Get all types inheriting from DbContext
        var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(DbContext).IsAssignableFrom(type)
                           && !type.IsInterface
                           && !type.IsAbstract);

        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;

            foreach (var dbContextType in dbContextTypes)
            {
                // Resolve the DbContext from the scoped service provider
                var dbContext = scopedServices.GetService(dbContextType) as DbContext;
                if (dbContext is null) continue;

                // Clear the database
                await dbContext.Database.EnsureDeletedAsync();
                // Recreate the database
                await dbContext.Database.EnsureCreatedAsync();
            }
        }
    }
}