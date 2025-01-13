using CliniCore.Modules.Appointments.Shell.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CliniCore.Modules.Appointments.Shell.DAL;
public class AppointmentsDbContext : DbContext
{
    public DbSet<AppointmentEntity> Appointments { get; set; }

    public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("appointments");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
