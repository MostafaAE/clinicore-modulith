using CliniCore.Modules.Bookings.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliniCore.Modules.Bookings.Infrastructure.DAL.Configuration;
internal class BookingConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.HasIndex(x => x.SlotId)
            .IsUnique();
    }
}
