using CliniCore.Modules.Appointments.Core.DTO;

namespace CliniCore.Modules.Appointments.Core.InputPorts;
public interface IAppointmentsService
{
    public Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync();
    Task UpdateAppointmentStatusAsync(Guid id, UpdateStatusCommand command);
}
