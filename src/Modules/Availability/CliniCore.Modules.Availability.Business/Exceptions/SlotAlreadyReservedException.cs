using CliniCore.Modules.Availability.Business.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Availability.Business.Exceptions;

internal class SlotAlreadyReservedException : ClinicoreException
{
    public SlotAlreadyReservedException()
        : base(ExceptionMessages.SlotAlreadyReserved, 400) { }
}