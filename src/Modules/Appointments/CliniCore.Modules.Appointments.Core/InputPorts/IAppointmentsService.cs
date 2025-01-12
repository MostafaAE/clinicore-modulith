using CliniCore.Modules.Appointments.Core.DTO;

namespace CliniCore.Modules.Appointments.Core.InputPorts;
public interface IAppointmentsService
{
    public Task<IEnumerable<AppointmentDto>> GetUpcomingAppointments();
}
