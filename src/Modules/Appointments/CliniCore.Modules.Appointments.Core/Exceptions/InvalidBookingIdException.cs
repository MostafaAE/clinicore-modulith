using CliniCore.Modules.Appointments.Core.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Exceptions;
public class InvalidBookingIdException : ClinicoreException
{
    public InvalidBookingIdException() 
        : base(ExceptionMessages.InvalidBookingId, 400) { }
}
