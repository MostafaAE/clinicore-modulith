using CliniCore.Modules.Bookings.Domain.Models;

namespace CliniCore.Modules.Bookings.Domain.Contracts;
public interface IBookingRepository
{
    Task<Guid> AddBooking(Booking booking);
}
