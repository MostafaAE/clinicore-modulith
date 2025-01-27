using CliniCore.Modules.Bookings.Application.Interfaces;

namespace CliniCore.Modules.Bookings.Application.GetAvailableBookings;
public class GetAvailableBookingsHandler
{
    private readonly IAvailabilityService _availabilityService;

    public GetAvailableBookingsHandler(IAvailabilityService availabilityModuleApi)
    {
        _availabilityService = availabilityModuleApi;
    }

    public async Task<IEnumerable<AvailableBookingDto>> Handle()
    {
        var availableBookings = await _availabilityService.GetAvailableSlotsAsync();

        return availableBookings;
    }
}
