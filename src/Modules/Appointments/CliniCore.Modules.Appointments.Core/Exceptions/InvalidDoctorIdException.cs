using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class InvalidDoctorIdException : ClinicoreException
{
    public InvalidDoctorIdException() 
        : base(ExceptionMessages.InvalidDoctorId, 400) { }
}
