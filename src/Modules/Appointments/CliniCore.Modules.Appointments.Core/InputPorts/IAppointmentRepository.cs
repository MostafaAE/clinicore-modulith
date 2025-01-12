using CliniCore.Modules.Appointments.Core.Models;

namespace CliniCore.Modules.Appointments.Core.InputPorts;

public interface IAppointmentRepository
{
    Task<Guid> AddAppointmentAsync(Appointment appointment);
}