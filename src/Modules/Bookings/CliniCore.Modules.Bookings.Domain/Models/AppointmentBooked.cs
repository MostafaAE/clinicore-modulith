namespace CliniCore.Modules.Bookings.Domain.Models;
public record AppointmentBooked(Guid SlotId, DateTime AppointmentTime, Guid DoctorId, string DoctorName,
    Guid PatientId, string PatientName, DateTime ReservedAt, decimal Cost) : IBookingEvent
{
}
