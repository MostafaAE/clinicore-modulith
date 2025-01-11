using AutoFixture;
using CliniCore.Modules.Availability.Business.EventHandlers.Booking;
using CliniCore.Modules.Availability.Business.Exceptions;
using CliniCore.Modules.Availability.Business.Services;
using CliniCore.Modules.Availability.Data;
using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Modules.Bookings.Shared;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Tests.Availability.Business.Services.EventHandlers.Booking;
public class AppointmentBookedEventHandlerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;
    private readonly AppointmentBookedEventHandler _handler;

    public AppointmentBookedEventHandlerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        var slotsService = _factory.Services.GetRequiredService<SlotsService>();
        _handler = new AppointmentBookedEventHandler(slotsService);
    }

    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task HandleAsync_ShouldReserveSlot_WhenEventIsPublished()
    {
        // Arrange
        var slotId = Guid.NewGuid();
        var slot = _fixture.Build<SlotEntity>()
            .With(slot => slot.Id, slotId)
            .With(slot => slot.IsReserved, false)
            .Create();

        await TestUtils.AddToDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slot);

        var appointmentBookedEvent = _fixture.Build<AppointmentBookedDto>()
            .With(e => e.SlotId, slotId)
            .Create();

        // Act
        await _handler.HandleAsync(appointmentBookedEvent);

        // Assert
        var updatedSlot = await TestUtils.GetFromDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slotId);
        updatedSlot.Should().NotBeNull();
        updatedSlot.IsReserved.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowSlotAlreadyReservedException_WhenSlotIsAlreadyReserved()
    {
        // Arrange
        var slotId = Guid.NewGuid();
        var slot = _fixture.Build<SlotEntity>()
            .With(slot => slot.Id, slotId)
            .With(slot => slot.IsReserved, true)
            .Create();

        await TestUtils.AddToDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slot);

        var appointmentBookedEvent = _fixture.Build<AppointmentBookedDto>()
            .With(e => e.SlotId, slotId)
            .Create();

        // Act
        Func<Task> act = async () => await _handler.HandleAsync(appointmentBookedEvent);

        // Assert
        await act.Should().ThrowAsync<SlotAlreadyReservedException>();
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowSlotNotFoundException_WhenSlotDoesNotExist()
    {
        // Arrange
        var slotId = Guid.NewGuid();
        var appointmentBookedEvent = _fixture.Build<AppointmentBookedDto>()
            .With(e => e.SlotId, slotId)
            .Create();

        // Act
        Func<Task> act = async () => await _handler.HandleAsync(appointmentBookedEvent);

        // Assert
        await act.Should().ThrowAsync<SlotNotFoundException>();
    }
}
