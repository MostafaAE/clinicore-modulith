using CliniCore.Modules.Bookings.Domain.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Bookings.Domain.Exceptions;
internal class InvalidSlotIdException : ClinicoreException
{
    public InvalidSlotIdException()
        : base(ExceptionMessages.InvalidSlotId, 400) { }
}
