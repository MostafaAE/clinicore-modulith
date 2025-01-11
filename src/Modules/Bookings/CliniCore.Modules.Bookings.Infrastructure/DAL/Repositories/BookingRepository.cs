using CliniCore.Modules.Bookings.Domain.Contracts;
using CliniCore.Modules.Bookings.Domain.Models;
using CliniCore.Modules.Bookings.Infrastructure.DAL.Entities;
using CliniCore.Modules.Bookings.Infrastructure.EventPublishers;

namespace CliniCore.Modules.Bookings.Infrastructure.DAL.Repositories;
public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _dbContext;
    private readonly IBookingPublisher _bookingPublisher;

    public BookingRepository(BookingDbContext dbContext, IBookingPublisher bookingPublisher)
    {
        _dbContext = dbContext;
        _bookingPublisher = bookingPublisher;
    }

    public async Task<Guid> AddBooking(Booking booking)
    {
        var bookingEntity = BookingEntity.From(booking);

        await _dbContext.AddAsync(bookingEntity);
        await _dbContext.SaveChangesAsync();

        // We have here dual-write problem, for real case scenario we should use the outbox pattern
        foreach (var bookingEvent in booking.DomainEvents)
        {
            await _bookingPublisher.PublishAsync(bookingEvent);
        }
        return bookingEntity.Id;
    }
}
