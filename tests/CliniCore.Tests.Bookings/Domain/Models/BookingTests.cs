using AutoFixture;
using CliniCore.Modules.Bookings.Domain.Exceptions;
using CliniCore.Modules.Bookings.Domain.Models;
using FluentAssertions;

namespace CliniCore.Tests.Bookings.Domain.Models;
public class BookingTests
{
    private readonly Fixture _fixture;

    public BookingTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void NewBooking_ShouldCreateBookingAndAddDomainEvent()
    {
        // Arrange
        var slotId = Guid.NewGuid();
        var appointmentTime = DateTime.UtcNow.AddHours(2);
        var doctorId = Guid.NewGuid();
        var doctorName = _fixture.Create<string>();
        var patientId = Guid.NewGuid();
        var patientName = _fixture.Create<string>();
        var reservedAt = DateTime.UtcNow;
        var cost = _fixture.Create<decimal>();

        // Act
        var booking = Booking.NewBooking(slotId, appointmentTime, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        booking.Should().NotBeNull();
        booking.SlotId.Should().Be(slotId);
        booking.PatientId.Should().Be(patientId);
        booking.PatientName.Should().Be(patientName);
        booking.ReservedAt.Should().Be(reservedAt);

        booking.DomainEvents.Should().HaveCount(1);
        var domainEvent = booking.DomainEvents.First();
        domainEvent.Should().BeOfType<AppointmentBooked>();

        var appointmentBooked = domainEvent as AppointmentBooked;
        appointmentBooked.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldInitializeBooking()
    {
        // Arrange
        var id = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var patientName = _fixture.Create<string>();
        var reservedAt = DateTime.UtcNow;

        // Act
        var booking = Booking.Create(id, slotId, patientId, patientName, reservedAt);

        // Assert
        booking.Should().NotBeNull();
        booking.SlotId.Should().Be(slotId);
        booking.PatientId.Should().Be(patientId);
        booking.PatientName.Should().Be(patientName);
        booking.ReservedAt.Should().Be(reservedAt);
        booking.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void NewBooking_ShouldThrowInvalidSlotIdException_WhenSlotIdIsEmpty()
    {
        // Arrange
        var slotId = Guid.Empty;
        var appointmentTime = DateTime.UtcNow.AddHours(2);
        var doctorId = Guid.NewGuid();
        var doctorName = _fixture.Create<string>();
        var patientId = Guid.NewGuid();
        var patientName = _fixture.Create<string>();
        var reservedAt = DateTime.UtcNow;
        var cost = _fixture.Create<decimal>();

        // Act
        Action act = () => Booking.NewBooking(slotId, appointmentTime, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        act.Should().Throw<InvalidSlotIdException>();
    }

    [Fact]
    public void NewBooking_ShouldThrowInvalidPatientIdException_WhenPatientIdIsEmpty()
    {
        // Arrange
        var slotId = Guid.NewGuid();
        var appointmentTime = DateTime.UtcNow.AddHours(2);
        var doctorId = Guid.NewGuid();
        var doctorName = _fixture.Create<string>();
        var patientId = Guid.Empty;
        var patientName = _fixture.Create<string>();
        var reservedAt = DateTime.UtcNow;
        var cost = _fixture.Create<decimal>();

        // Act
        Action act = () => Booking.NewBooking(slotId, appointmentTime, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        act.Should().Throw<InvalidPatientIdException>();
    }
}
