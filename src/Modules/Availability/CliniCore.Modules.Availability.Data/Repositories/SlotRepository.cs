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

    public async Task<IEnumerable<SlotEntity>> GetAvailableSlotsAsync()
    {
        // Assume that available slots have the following criteria
        // IsReserved = false & Time >= now
        var slots =
                    await _context.Slots
                                  .Where(slot => slot.IsReserved == false &&
                                                 slot.Time >= _clock.CurrentDate())
                                  .ToListAsync();

        return slots;
    }

    public async Task<Guid> AddSlotAsync(SlotEntity slot)
    {
        await _context.Slots.AddAsync(slot);
        await _context.SaveChangesAsync();

        return slot.Id;
    }

    public async Task<SlotEntity> GetSlotByIdAsync(Guid id)
    {
        var slot = await _context.Slots.FindAsync(id);
        return slot;
    }
}
