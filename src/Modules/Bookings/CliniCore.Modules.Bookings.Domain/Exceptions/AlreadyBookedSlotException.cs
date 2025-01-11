using CliniCore.Modules.Bookings.Domain.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Bookings.Domain.Exceptions;
public class AlreadyBookedSlotException : ClinicoreException
{
    public AlreadyBookedSlotException()
        : base(ExceptionMessages.AlreadyBookedSlot, 400) { }
}
