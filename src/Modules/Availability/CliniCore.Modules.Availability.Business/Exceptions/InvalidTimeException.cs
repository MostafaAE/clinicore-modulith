using CliniCore.Modules.Availability.Business.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Availability.Business.Exceptions;
internal class InvalidSlotTimeException : ClinicoreException
{
    public InvalidSlotTimeException()
        : base(ExceptionMessages.InvalidTime, 400) { }
}
