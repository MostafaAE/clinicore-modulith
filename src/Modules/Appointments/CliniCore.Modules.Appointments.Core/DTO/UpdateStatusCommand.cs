using CliniCore.Modules.Appointments.Core.Models;

namespace CliniCore.Modules.Appointments.Core.DTO;

public class UpdateStatusCommand
{
    public AppointmentStatus Status { get; set; }
}