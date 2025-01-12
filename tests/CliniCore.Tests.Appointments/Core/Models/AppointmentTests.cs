using CliniCore.Modules.Appointments.Core.Exceptions;
using CliniCore.Modules.Appointments.Core.Models;
using FluentAssertions;

namespace CliniCore.Tests.Appointments.Core.Models;
public class AppointmentTests
{
    [Fact]
    public void NewAppointment_ShouldCreateValidAppointment()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var time = DateTime.UtcNow.AddDays(1);
        var doctorId = Guid.NewGuid();
        var doctorName = "Dr. John Doe";
        var patientId = Guid.NewGuid();
        var patientName = "Jane Doe";
        var reservedAt = DateTime.UtcNow;
        var cost = 100.50m;

        // Act
        var appointment = Appointment.NewAppointment(bookingId, slotId, time, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        appointment.Should().NotBeNull();
        appointment.BookingId.Should().Be(bookingId);
        appointment.SlotId.Should().Be(slotId);
        appointment.Time.Should().Be(time);
        appointment.DoctorId.Should().Be(doctorId);
        appointment.DoctorName.Should().Be(doctorName);
        appointment.PatientId.Should().Be(patientId);
        appointment.PatientName.Should().Be(patientName);
        appointment.ReservedAt.Should().Be(reservedAt);
        appointment.Cost.Should().Be(cost);
    }

    [Fact]
    public void Create_ShouldThrowInvalidBookingIdException_WhenBookingIdIsEmpty()
    {
        // Arrange
        var slotId = Guid.NewGuid();
        var time = DateTime.UtcNow.AddDays(1);
        var doctorId = Guid.NewGuid();
        var doctorName = "Dr. John Doe";
        var patientId = Guid.NewGuid();
        var patientName = "Jane Doe";
        var reservedAt = DateTime.UtcNow;
        var cost = 100.50m;

        // Act
        Action act = () => Appointment.Create(Guid.NewGuid(), Guid.Empty, slotId, time, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        act.Should().Throw<InvalidBookingIdException>();
    }

    [Fact]
    public void Create_ShouldThrowInvalidSlotIdException_WhenSlotIdIsEmpty()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var time = DateTime.UtcNow.AddDays(1);
        var doctorId = Guid.NewGuid();
        var doctorName = "Dr. John Doe";
        var patientId = Guid.NewGuid();
        var patientName = "Jane Doe";
        var reservedAt = DateTime.UtcNow;
        var cost = 100.50m;

        // Act
        Action act = () => Appointment.Create(Guid.NewGuid(), bookingId, Guid.Empty, time, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        act.Should().Throw<InvalidSlotIdException>();
    }

    [Fact]
    public void Create_ShouldThrowInvalidDoctorIdException_WhenDoctorIdIsEmpty()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var time = DateTime.UtcNow.AddDays(1);
        var doctorName = "Dr. John Doe";
        var patientId = Guid.NewGuid();
        var patientName = "Jane Doe";
        var reservedAt = DateTime.UtcNow;
        var cost = 100.50m;

        // Act
        Action act = () => Appointment.Create(Guid.NewGuid(), bookingId, slotId, time, Guid.Empty, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        act.Should().Throw<InvalidDoctorIdException>();
    }

    [Fact]
    public void Create_ShouldThrowInvalidPatientIdException_WhenPatientIdIsEmpty()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var time = DateTime.UtcNow.AddDays(1);
        var doctorId = Guid.NewGuid();
        var doctorName = "Dr. John Doe";
        var patientName = "Jane Doe";
        var reservedAt = DateTime.UtcNow;
        var cost = 100.50m;

        // Act
        Action act = () => Appointment.Create(Guid.NewGuid(), bookingId, slotId, time, doctorId, doctorName, Guid.Empty, patientName, reservedAt, cost);

        // Assert
        act.Should().Throw<InvalidPatientIdException>();
    }

    [Fact]
    public void Create_ShouldInitializeCorrectly_WhenValidParametersProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var time = DateTime.UtcNow.AddDays(1);
        var doctorId = Guid.NewGuid();
        var doctorName = "Dr. John Doe";
        var patientId = Guid.NewGuid();
        var patientName = "Jane Doe";
        var reservedAt = DateTime.UtcNow;
        var cost = 100.50m;

        // Act
        var appointment = Appointment.Create(id, bookingId, slotId, time, doctorId, doctorName, patientId, patientName, reservedAt, cost);

        // Assert
        appointment.Id.Should().Be(id);
        appointment.BookingId.Should().Be(bookingId);
        appointment.SlotId.Should().Be(slotId);
        appointment.Time.Should().Be(time);
        appointment.DoctorId.Should().Be(doctorId);
        appointment.DoctorName.Should().Be(doctorName);
        appointment.PatientId.Should().Be(patientId);
        appointment.PatientName.Should().Be(patientName);
        appointment.ReservedAt.Should().Be(reservedAt);
        appointment.Cost.Should().Be(cost);
    }
}
