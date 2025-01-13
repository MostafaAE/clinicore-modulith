using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class AppointmentAlreadyCompletedOrCanceledFoundException : ClinicoreException
{
    public AppointmentAlreadyCompletedOrCanceledFoundException() 
        : base(ExceptionMessages.AppointmentAlreadyCompletedOrCanceled, 400) { }
}
