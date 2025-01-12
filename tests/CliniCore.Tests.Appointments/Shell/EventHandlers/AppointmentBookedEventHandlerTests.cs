using AutoFixture;
using CliniCore.Modules.Appointments.Core.Exceptions;
using CliniCore.Modules.Appointments.Core.InputPorts;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Shell.DAL;
using CliniCore.Modules.Appointments.Shell.DAL.Entities;
using CliniCore.Modules.Appointments.Shell.EventHandlers;
using CliniCore.Modules.Bookings.Shared;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Tests.Appointments.Shell.EventHandlers;
public class AppointmentBookedEventHandlerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;
    private readonly AppointmentBookedEventHandler _handler;
    private readonly IAppointmentRepository _repository;

    public AppointmentBookedEventHandlerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();

        // Resolve dependencies from the test application container
        _repository = _factory.Services.GetRequiredService<IAppointmentRepository>();
        _handler = new AppointmentBookedEventHandler(_repository);
    }

    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task HandleAsync_ShouldAddAppointment_WhenEventIsValid()
    {
        // Arrange
        var appointmentBookedEvent = _fixture.Build<AppointmentBookedDto>()
            .With(e => e.BookingId, Guid.NewGuid())
            .With(e => e.SlotId, Guid.NewGuid())
            .With(e => e.DoctorId, Guid.NewGuid())
            .With(e => e.PatientId, Guid.NewGuid())
            .With(e => e.AppointmentTime, DateTime.UtcNow.AddDays(1))
            .With(e => e.ReservedAt, DateTime.UtcNow)
            .With(e => e.Cost, 200.00m)
            .Create();

        // Act
        await _handler.HandleAsync(appointmentBookedEvent);

        // Assert
        var appointments = await TestUtils.GetFromDatabaseAsync<AppointmentsDbContext, AppointmentEntity>(
            _factory,
            entity => entity.BookingId == appointmentBookedEvent.BookingId
        );

        appointments.Should().HaveCount( 1 );

        var appointment = appointments.FirstOrDefault();
        appointment.Should().NotBeNull();
        appointment.BookingId.Should().Be(appointmentBookedEvent.BookingId);
        appointment.SlotId.Should().Be(appointmentBookedEvent.SlotId);
        appointment.DoctorId.Should().Be(appointmentBookedEvent.DoctorId);
        appointment.PatientId.Should().Be(appointmentBookedEvent.PatientId);
        appointment.Cost.Should().Be(appointmentBookedEvent.Cost);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenSlotIdIsInvalid()
    {
        // Arrange
        var appointmentBookedEvent = _fixture.Build<AppointmentBookedDto>()
            .With(e => e.SlotId, Guid.Empty) // Invalid SlotId
            .Create();

        // Act
        Func<Task> act = async () => await _handler.HandleAsync(appointmentBookedEvent);

        // Assert
        await act.Should().ThrowAsync<InvalidSlotIdException>();
    }
}
