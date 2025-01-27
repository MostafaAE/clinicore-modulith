using CliniCore.Modules.Bookings.Application.Interfaces;
using CliniCore.Modules.Bookings.Domain.Contracts;
using CliniCore.Modules.Bookings.Domain.Exceptions;
using CliniCore.Modules.Bookings.Domain.Models;
using CliniCore.Shared.Time;

namespace CliniCore.Modules.Bookings.Application.BookAppointment;
public class AddBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IAvailabilityService _availabilityService;
    private readonly IClock _clock;

    public AddBookingHandler(IBookingRepository bookingRepository, IAvailabilityService availabilityModuleApi, IClock clock)
    {
        _availabilityService = availabilityModuleApi;
        _clock = clock;
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingResponse> Handle(AddBooking command)
    {
        var slot = await _availabilityService.GetSlotByIdAsync(command.SlotId);

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
