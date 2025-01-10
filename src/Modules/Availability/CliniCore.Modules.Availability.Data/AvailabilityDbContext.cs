using CliniCore.Modules.Availability.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CliniCore.Modules.Availability.Data;
public class AvailabilityDbContext : DbContext
{
    public DbSet<SlotEntity> Slots { get; set; }

    public AvailabilityDbContext(DbContextOptions<AvailabilityDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("availability");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
