using CliniCore.Modules.Bookings.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CliniCore.Modules.Bookings.Infrastructure.DAL;
public class BookingDbContext : DbContext
{
    public DbSet<BookingEntity> Bookings { get; set; }
    public BookingDbContext(DbContextOptions<BookingDbContext> options) 
        : base(options){ }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("booking");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
