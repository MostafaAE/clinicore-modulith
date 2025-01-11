using CliniCore.Modules.Bookings.Domain.Exceptions;
using CliniCore.Shared.Events;

namespace CliniCore.Modules.Bookings.Domain.Models;
public class Booking
{
    public Guid Id { get; private set; }
    public Guid SlotId { get; private set; }
    public Guid PatientId { get; private set; }
    public string PatientName { get; private set; }
    public DateTime ReservedAt { get; private set; }
    private readonly List<IBookingEvent> _domainEvents = new();
    public IReadOnlyList<IBookingEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Booking(Guid id, Guid slotId, Guid patientId, string patientName, DateTime reservedAt)
    {
        Id = id;

        if(slotId == Guid.Empty)
        {
            throw new InvalidSlotIdException();
        }

        SlotId = slotId;

        if(patientId == Guid.Empty)
        {
            throw new InvalidPatientIdException();
        }

        PatientId = patientId;

        PatientName = patientName;
        ReservedAt = reservedAt;
    }
    private void AddDomainEvent(IBookingEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public static Booking NewBooking(Guid slotId, DateTime appointmentTime, Guid doctorId, string doctorName,
                                    Guid patientId, string patientName, DateTime reservedAt, decimal cost)
    {
        var booking = new Booking(Guid.NewGuid(), slotId, patientId, patientName, reservedAt);

        var apppointmentBooked = new AppointmentBooked(slotId, appointmentTime, doctorId, 
            doctorName, patientId, patientName, reservedAt, cost);

        booking.AddDomainEvent(apppointmentBooked);

        return booking;
    }

    public static Booking Create(Guid id, Guid slotId, Guid patientId, string patientName, DateTime reservedAt)
    {
        return new Booking(Guid.NewGuid(), slotId, patientId, patientName, reservedAt);
    }
}
