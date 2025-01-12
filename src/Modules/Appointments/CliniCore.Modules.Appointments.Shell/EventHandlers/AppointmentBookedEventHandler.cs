using CliniCore.Modules.Appointments.Core.InputPorts;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Bookings.Shared;
using CliniCore.Shared.Events;

namespace CliniCore.Modules.Appointments.Shell.EventHandlers;
internal class AppointmentBookedEventHandler : IEventHandler<AppointmentBookedDto>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentBookedEventHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task HandleAsync(AppointmentBookedDto @event, CancellationToken cancellationToken = default)
    {
        var appointment = Appointment.NewAppointment(@event.BookingId, @event.SlotId, @event.AppointmentTime,
            @event.DoctorId, @event.DoctorName, @event.PatientId, @event.PatientName, @event.ReservedAt, @event.Cost);

        await _appointmentRepository.AddAppointmentAsync(appointment);
    }
}
