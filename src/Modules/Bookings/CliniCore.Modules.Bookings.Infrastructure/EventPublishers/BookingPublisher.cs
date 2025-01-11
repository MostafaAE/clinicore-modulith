using CliniCore.Modules.Bookings.Domain.Models;
using CliniCore.Modules.Bookings.Shared;
using CliniCore.Shared.Messaging;

namespace CliniCore.Modules.Bookings.Infrastructure.EventPublishers;
public class BookingPublisher : IBookingPublisher
{
    private readonly IMessageBroker _messageBroker;

    public BookingPublisher(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    public async Task PublishAsync(IBookingEvent bookingEvent)
    {
        if(bookingEvent is AppointmentBooked )
        {
            var appointementEvent = bookingEvent as AppointmentBooked;

            var integrationEvent = new AppointmentBookedDto(
                appointementEvent.SlotId,
                appointementEvent.AppointmentTime,
                appointementEvent.DoctorId,
                appointementEvent.DoctorName,
                appointementEvent.PatientId,
                appointementEvent.PatientName,
                appointementEvent.ReservedAt,
                appointementEvent.Cost);

            await _messageBroker.PublishAsync(integrationEvent);
        }
        
    }
}
