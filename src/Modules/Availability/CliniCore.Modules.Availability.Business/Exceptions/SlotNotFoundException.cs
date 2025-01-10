using CliniCore.Modules.Availability.Business.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Availability.Business.Exceptions;

internal class SlotNotFoundException : ClinicoreException
{
    public SlotNotFoundException()
        : base(ExceptionMessages.SlotNotFound, 404) { }
}