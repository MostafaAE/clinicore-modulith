using CliniCore.Modules.Availability.Data.Entities;

namespace CliniCore.Modules.Availability.Data.Repositories;
public interface ISlotRepository
{
    Task<IEnumerable<SlotEntity>> GetAllSlotsAsync();

    Task<Guid> AddSlotAsync(SlotEntity slot);
    Task<SlotEntity> GetSlotByIdAsync(Guid id);
}
