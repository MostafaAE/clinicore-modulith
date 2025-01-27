using AutoFixture;
using CliniCore.Modules.Bookings.Application.BookAppointment;
using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using CliniCore.Modules.Bookings.Application.Interfaces;
using CliniCore.Modules.Bookings.Domain.Contracts;
using CliniCore.Modules.Bookings.Domain.Exceptions;
using CliniCore.Modules.Bookings.Domain.Models;
using CliniCore.Shared.Time;
using FluentAssertions;
using Moq;

namespace CliniCore.Tests.Bookings.Application.BookAppointment;
public class AddBookingHandlerTests
{
    private readonly Mock<IAvailabilityService> _availabilityServiceMock;
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IClock> _clockMock;
    private readonly AddBookingHandler _handler;
    private readonly Fixture _fixture;

    public AddBookingHandlerTests()
    {
        _availabilityServiceMock = new Mock<IAvailabilityService>();
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _clockMock = new Mock<IClock>();
        _handler = new AddBookingHandler(_bookingRepositoryMock.Object, _availabilityServiceMock.Object, _clockMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ShouldReturnBookingResponse_WhenSlotIsAvailable()
    {
        // Arrange
        var command = _fixture.Create<AddBooking>();
        var slot = new AvailableBookingDto
        {
            Id = command.SlotId,
            IsReserved = false,
            Time = DateTime.UtcNow.AddHours(2),
            DoctorId = Guid.NewGuid(),
            DoctorName = _fixture.Create<string>(),
            Cost = 200m
        };

        _availabilityServiceMock
            .Setup(api => api.GetSlotByIdAsync(command.SlotId))
            .ReturnsAsync(slot);


        var expectedBookingId = Guid.NewGuid();
        _bookingRepositoryMock
            .Setup(repo => repo.AddBooking(It.IsAny<Booking>()))
            .ReturnsAsync(expectedBookingId);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedBookingId);

        _availabilityServiceMock.Verify(api => api.GetSlotByIdAsync(command.SlotId), Times.Once);
        _bookingRepositoryMock.Verify(repo => repo.AddBooking(It.IsAny<Booking>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyBookedSlotException_WhenSlotIsReserved()
    {
        // Arrange
        var command = _fixture.Create<AddBooking>();
        var slot = new AvailableBookingDto
        {
            Id = command.SlotId,
            IsReserved = true
        };

        _availabilityServiceMock
            .Setup(api => api.GetSlotByIdAsync(command.SlotId))
            .ReturnsAsync(slot);

        // Act
        Func<Task> act = async () => await _handler.Handle(command);

        // Assert
        await act.Should().ThrowAsync<AlreadyBookedSlotException>();

        _availabilityServiceMock.Verify(api => api.GetSlotByIdAsync(command.SlotId), Times.Once);
        _bookingRepositoryMock.Verify(repo => repo.AddBooking(It.IsAny<Booking>()), Times.Never);
    }
}