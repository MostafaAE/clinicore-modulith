using CliniCore.Modules.Bookings.Domain.Models;

namespace CliniCore.Modules.Bookings.Infrastructure.EventPublishers;
public interface IBookingPublisher
{
    Task PublishAsync(IBookingEvent bookingEvent);
}
