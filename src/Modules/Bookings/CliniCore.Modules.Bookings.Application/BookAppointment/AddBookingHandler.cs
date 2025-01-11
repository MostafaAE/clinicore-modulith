using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Bookings.Domain.Contracts;
using CliniCore.Modules.Bookings.Domain.Exceptions;
using CliniCore.Modules.Bookings.Domain.Models;
using CliniCore.Shared.Time;

namespace CliniCore.Modules.Bookings.Application.BookAppointment;
public class AddBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IAvailabilityModuleApi _availabilityModuleApi;
    private readonly IClock _clock;

    public AddBookingHandler(IBookingRepository bookingRepository, IAvailabilityModuleApi availabilityModuleApi, IClock clock)
    {
        _availabilityModuleApi = availabilityModuleApi;
        _clock = clock;
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingResponse> Handle(AddBooking command)
    {
        var slot = await _availabilityModuleApi.GetSlotByIdAsync(command.SlotId);

        if(slot.IsReserved == true)
        {
            throw new AlreadyBookedSlotException();
        }

        var booking = Booking.NewBooking
        (
            command.SlotId,
            slot.Time,
            slot.DoctorId,
            slot.DoctorName,
            command.PatientId,
            command.PatientName,
            _clock.CurrentDate(),
            slot.Cost);

        var bookingId = await _bookingRepository.AddBooking(booking);

        return new BookingResponse() { Id = bookingId };
    }
}
