using CliniCore.Shared.Events;

namespace CliniCore.Modules.Bookings.Domain.Models;
internal record AppointmentBooked(Guid SlotId, DateTime AppointmentTime, Guid DoctorId, string DoctorName,
    Guid PatientId, string PatientName, DateTime ReservedAt, decimal Cost) : IEvent
{
}
