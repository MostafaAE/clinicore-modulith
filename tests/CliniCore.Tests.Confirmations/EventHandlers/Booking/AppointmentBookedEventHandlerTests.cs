using AutoFixture;
using CliniCore.Modules.Bookings.Shared;
using CliniCore.Modules.Confirmations.Api.EventHandlers.Booking;
using CliniCore.Modules.Confirmations.Api.Services;
using Moq;

namespace CliniCore.Tests.Confirmations.EventHandlers.Booking;
public class AppointmentBookedEventHandlerTests
{
    private readonly Mock<IConfirmationSender> _confirmationSenderMock;
    private readonly AppointmentBookedEventHandler _handler;
    private readonly IFixture _fixture;

    public AppointmentBookedEventHandlerTests()
    {
        _confirmationSenderMock = new Mock<IConfirmationSender>();
        _handler = new AppointmentBookedEventHandler(_confirmationSenderMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task HandleAsync_ShouldSendConfirmationsToPatientAndDoctor()
    {
        // Arrange
        var appointmentEvent = _fixture.Build<AppointmentBookedDto>()
            .With(e => e.PatientName, "John Doe")
            .With(e => e.DoctorName, "Dr. Smith")
            .With(e => e.AppointmentTime, DateTime.Now)
            .Create();

        var expectedTemplate = $"Appointment for patient: ${appointmentEvent.PatientName}, with doctor: ${appointmentEvent.DoctorName} is confirmed and will be held at ${appointmentEvent.AppointmentTime}.";

        // Act
        await _handler.HandleAsync(appointmentEvent);

        // Assert
        _confirmationSenderMock.Verify(sender =>
            sender.SendAsync(appointmentEvent.PatientName, expectedTemplate), Times.Once);

        _confirmationSenderMock.Verify(sender =>
            sender.SendAsync(appointmentEvent.DoctorName, expectedTemplate), Times.Once);
    }
}
