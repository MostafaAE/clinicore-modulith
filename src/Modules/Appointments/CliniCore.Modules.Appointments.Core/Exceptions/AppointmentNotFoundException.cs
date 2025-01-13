using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class AppointmentNotFoundException : ClinicoreException
{
    public AppointmentNotFoundException() 
        : base(ExceptionMessages.AppointmentNotFound, 404) { }
}
