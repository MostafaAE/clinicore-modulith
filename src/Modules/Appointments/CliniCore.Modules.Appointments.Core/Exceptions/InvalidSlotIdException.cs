using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class InvalidSlotIdException : ClinicoreException
{
    public InvalidSlotIdException() 
        : base(ExceptionMessages.InvalidSlotId, 400) { }
}
