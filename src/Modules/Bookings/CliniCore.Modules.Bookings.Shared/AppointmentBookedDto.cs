using CliniCore.Shared.Events;

namespace CliniCore.Modules.Bookings.Shared;
public record AppointmentBookedDto(Guid BookingId, Guid SlotId, DateTime AppointmentTime, Guid DoctorId, string DoctorName,
    Guid PatientId, string PatientName, DateTime ReservedAt, decimal Cost) : IEvent
{
}
