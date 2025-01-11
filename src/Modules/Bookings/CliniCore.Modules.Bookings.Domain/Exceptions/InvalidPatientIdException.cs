using CliniCore.Modules.Bookings.Domain.Resources;
using CliniCore.Shared.Exceptions;

namespace CliniCore.Modules.Bookings.Domain.Exceptions;
internal class InvalidPatientIdException : ClinicoreException
{
    public InvalidPatientIdException()
        : base(ExceptionMessages.InvalidPatientId, 400) { }
}
