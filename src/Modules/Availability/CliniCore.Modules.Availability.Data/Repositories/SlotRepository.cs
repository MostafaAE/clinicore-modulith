using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Shared.Time;
using Microsoft.EntityFrameworkCore;

namespace CliniCore.Modules.Availability.Data.Repositories;
public class SlotRepository : ISlotRepository
{
    private readonly AvailabilityDbContext _context;
    private readonly IClock _clock;

    public SlotRepository(AvailabilityDbContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<IEnumerable<SlotEntity>> GetAllSlotsAsync()
    {
        var slots = await _context.Slots.ToListAsync();

        return slots;
    }

    public async Task<SlotEntity> GetSlotByIdAsync(Guid id)
    {
        var slot = await _context.Slots.FindAsync(id);
        return slot;
    }
}
