using CliniCore.Modules.Availability.Business.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Availability.Business.Exceptions;
internal class InvalidSlotCostException : ClinicoreException
{
    public InvalidSlotCostException()
        : base(ExceptionMessages.InvalidCost, 400) { }
}
