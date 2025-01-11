using CliniCore.Modules.Availability.Shared.DTO;

namespace CliniCore.Modules.Availability.Shared;
public interface IAvailabilityModuleApi
{
    Task<IEnumerable<SlotDto>> GetAvailableSlotsAsync();

    Task<SlotDto> GetSlotByIdAsync(Guid id);
}
