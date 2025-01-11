using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;

namespace CliniCore.Modules.Bookings.Application.GetAvailableBookings;
public class GetAvailableBookingsHandler
{
    private readonly IAvailabilityModuleApi _availabilityModuleApi;

    public GetAvailableBookingsHandler(IAvailabilityModuleApi availabilityModuleApi)
    {
        _availabilityModuleApi = availabilityModuleApi;
    }

    public async Task<IEnumerable<AvailableBookingDto>> Handle()
    {
        var slots = await _availabilityModuleApi.GetAvailableSlotsAsync();

        var availableBookings = Map(slots);

        return availableBookings;
    }

    private IEnumerable<AvailableBookingDto> Map(IEnumerable<SlotDto> slotDtos)
    {
        var availableBookingsDto = slotDtos.Select(slotDto => new AvailableBookingDto()
        {
            Id = slotDto.Id,
            Cost = slotDto.Cost,
            DoctorId = slotDto.DoctorId,
            DoctorName = slotDto.DoctorName,
            IsReserved = slotDto.IsReserved,
            Time = slotDto.Time,
        });

        return availableBookingsDto;
    }
}
