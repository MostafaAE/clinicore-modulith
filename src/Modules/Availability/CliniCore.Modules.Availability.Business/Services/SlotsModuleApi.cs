using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;

namespace CliniCore.Modules.Availability.Business.Services;
internal class SlotsModuleApi : ISlotsModuleApi
{
    private readonly SlotsService _slotsService;

    public SlotsModuleApi(SlotsService slotsService)
    {
        _slotsService = slotsService;
    }

    public async Task<IEnumerable<SlotDto>> GetAvailableSlotsAsync()
    {
        return await _slotsService.GetAvailableSlotsAsync();
    }

    public async Task<SlotDto> GetSlotByIdAsync(Guid id)
    {
        return await _slotsService.GetSlotByIdAsync(id);
    }
}
