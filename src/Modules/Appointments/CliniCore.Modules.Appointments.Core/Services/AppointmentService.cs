using CliniCore.Modules.Appointments.Core.DTO;
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

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointments()
    {
        var appointments = await _appointmentRepository.GetUpcomingAppointmentsAsync();

        var dtos = Map(appointments);

        return dtos;
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
