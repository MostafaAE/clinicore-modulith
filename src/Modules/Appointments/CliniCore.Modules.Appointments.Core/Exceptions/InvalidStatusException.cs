using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class InvalidStatusException : ClinicoreException
{
    public InvalidStatusException() 
        : base(ExceptionMessages.InvalidStatus, 400) { }
}
