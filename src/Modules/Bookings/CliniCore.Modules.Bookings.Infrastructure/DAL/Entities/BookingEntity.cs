using CliniCore.Modules.Bookings.Domain.Models;

namespace CliniCore.Modules.Bookings.Infrastructure.DAL.Entities;
public class BookingEntity
{
    public Guid Id { get; set; }
    public Guid SlotId {  get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; }
    public DateTime ReservedAt { get; set; }

    public static BookingEntity From(Booking booking)
    {
        return new BookingEntity() {
            Id = booking.Id,
            SlotId = booking.SlotId,
            PatientId = booking.PatientId,
            PatientName = booking.PatientName,
            ReservedAt = booking.ReservedAt 
        };
    }

    public Booking ToDomain()
    {
        return Booking.Create(Id, SlotId, PatientId, PatientName, ReservedAt);
    }
}
