using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class InvalidPatientIdException : ClinicoreException
{
    public InvalidPatientIdException() 
        : base(ExceptionMessages.InvalidPatientId, 400) { }
}
