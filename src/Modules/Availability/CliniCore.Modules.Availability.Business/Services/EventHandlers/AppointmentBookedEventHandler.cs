using CliniCore.Modules.Bookings.Shared;
using CliniCore.Shared.Events;

namespace CliniCore.Modules.Availability.Business.Services.EventHandlers;
internal class AppointmentBookedEventHandler : IEventHandler<AppointmentBookedDto>
{
    private readonly SlotsService _slotsService;

    public AppointmentBookedEventHandler(SlotsService slotsService)
    {
        _slotsService = slotsService;
    }

    public async Task HandleAsync(AppointmentBookedDto @event, CancellationToken cancellationToken = default)
    {
        await _slotsService.ReserveSlotAsync(@event.SlotId);
    }
}
