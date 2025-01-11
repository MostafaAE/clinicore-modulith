using CliniCore.Modules.Bookings.Shared;
using CliniCore.Modules.Confirmations.Api.Services;
using CliniCore.Shared.Events;

namespace CliniCore.Modules.Confirmations.Api.EventHandlers.Booking;
internal class AppointmentBookedEventHandler : IEventHandler<AppointmentBookedDto>
{
    private readonly IConfirmationSender _confirmationSender;
public AppointmentBookedEventHandler(IConfirmationSender confirmationSender)
    {
        _confirmationSender = confirmationSender;
    }

    public async Task HandleAsync(AppointmentBookedDto @event, CancellationToken cancellationToken = default)
    {
        var template = $"Appointment for patient: ${@event.PatientName}, with doctor: ${@event.DoctorName} is confirmed and will be held at ${@event.AppointmentTime}.";
        await _confirmationSender.SendAsync(@event.PatientName, template);
        await _confirmationSender.SendAsync(@event.DoctorName, template);
    }
}
