using CliniCore.Modules.Bookings.Application.GetAvailableBookings;

namespace CliniCore.Modules.Bookings.Application.Interfaces;
public interface IAvailabilityService
{
    Task<IEnumerable<AvailableBookingDto>> GetAvailableSlotsAsync();
    Task<AvailableBookingDto> GetSlotByIdAsync(Guid id);
}
