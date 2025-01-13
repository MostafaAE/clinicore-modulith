using CliniCore.Modules.Appointments.Core.DTO;
using CliniCore.Modules.Appointments.Core.Exceptions;
using CliniCore.Modules.Appointments.Core.InputPorts;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Core.OutputPorts;

namespace CliniCore.Modules.Appointments.Core.Services;
public class AppointmentService : IAppointmentsService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetUpcomingAppointmentsAsync();

        var dtos = Map(appointments);

        return dtos;
    }

    public async Task UpdateAppointmentStatusAsync(Guid id, UpdateStatusCommand command)
    {
        var appointmentModel = await _appointmentRepository.GetAppointmentByIdAsync(id);

        if (appointmentModel is null)
            throw new AppointmentNotFoundException();

        if(appointmentModel.Status != AppointmentStatus.Booked)
            throw new AppointmentAlreadyCompletedOrCanceledFoundException();

        if (command.Status is AppointmentStatus.Completed)
        {
            appointmentModel.Complete();
        }
        else if (command.Status is AppointmentStatus.Canceled)
        {
            appointmentModel.Cancel();
        }

        await _appointmentRepository.UpdateAppointmentAsync(appointmentModel);
    }

    private IEnumerable<AppointmentDto> Map(IEnumerable<Appointment> appointments)
    {
        return appointments.Select(appointment => new AppointmentDto
        {
            Id = appointment.Id,
            BookingId = appointment.BookingId,
            SlotId = appointment.SlotId,
            Time = appointment.Time,
            DoctorId = appointment.DoctorId,
            DoctorName = appointment.DoctorName,
            PatientId = appointment.PatientId,
            PatientName = appointment.PatientName,
            ReservedAt = appointment.ReservedAt,
            Cost = appointment.Cost,
            Status = appointment.Status,
        });
    }
}
