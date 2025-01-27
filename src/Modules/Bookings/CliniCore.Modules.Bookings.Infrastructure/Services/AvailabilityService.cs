using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using CliniCore.Modules.Bookings.Application.Interfaces;

namespace CliniCore.Modules.Bookings.Infrastructure.Services;
internal class AvailabilityService : IAvailabilityService
{
    private readonly IAvailabilityModuleApi _availabilityModuleApi;

    public AvailabilityService(IAvailabilityModuleApi availabilityModuleApi)
    {
        _availabilityModuleApi = availabilityModuleApi;
    }

    public async Task<IEnumerable<AvailableBookingDto>> GetAvailableSlotsAsync()
    {
        var slots = await _availabilityModuleApi.GetAvailableSlotsAsync();

        var availableBookings = Map(slots);

        return availableBookings;
    }

    public async Task<AvailableBookingDto> GetSlotByIdAsync(Guid id)
    {
        var slot = await _availabilityModuleApi.GetSlotByIdAsync(id);

        var availableBooking = Map(slot);

        return availableBooking;
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

    private AvailableBookingDto Map(SlotDto slotDto)
    {
        var availableBookingDto = new AvailableBookingDto()
        {
            Id = slotDto.Id,
            Cost = slotDto.Cost,
            DoctorId = slotDto.DoctorId,
            DoctorName = slotDto.DoctorName,
            IsReserved = slotDto.IsReserved,
            Time = slotDto.Time,
        };

        return availableBookingDto;
    }

}
