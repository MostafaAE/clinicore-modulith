using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace CliniCore.Tests.Shared.Utils;

public static class TestUtils
{
    // Add a single entity to a specific DbContext
    public static async Task AddToDatabaseAsync<TContext, TEntity>(CustomWebApplicationFactory factory, TEntity entity)
        where TContext : DbContext
        where TEntity : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        dbContext.Set<TEntity>().Add(entity);
        await dbContext.SaveChangesAsync();
    }

    // Add multiple entities to a specific DbContext
    public static async Task AddToDatabaseAsync<TContext, TEntity>(CustomWebApplicationFactory factory, IEnumerable<TEntity> entities)
        where TContext : DbContext
        where TEntity : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        dbContext.Set<TEntity>().AddRange(entities);
        await dbContext.SaveChangesAsync();
    }

    // Retrieve an entity by ID from a specific DbContext
    public static async Task<TEntity> GetFromDatabaseAsync<TContext, TEntity>(CustomWebApplicationFactory factory, Guid id)
        where TContext : DbContext
        where TEntity : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        return await dbContext.Set<TEntity>().FindAsync(id);
    }

    public static async Task<IEnumerable<TEntity>> GetFromDatabaseAsync<TContext, TEntity>(CustomWebApplicationFactory factory, Expression<Func<TEntity, bool>> filter)
        where TContext : DbContext
        where TEntity : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        return await dbContext.Set<TEntity>().Where(filter).ToListAsync();
    }

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