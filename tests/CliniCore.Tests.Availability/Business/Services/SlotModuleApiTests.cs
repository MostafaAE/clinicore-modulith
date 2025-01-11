using AutoFixture;
using CliniCore.Modules.Availability.Business.Exceptions;
using CliniCore.Modules.Availability.Data;
using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Modules.Availability.Shared;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CliniCore.Tests.Availability.Business.Services;

public class SlotsModuleApiTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IAvailabilityModuleApi _slotsModuleApi;
    private readonly IFixture _fixture;

    public SlotsModuleApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _slotsModuleApi = factory.Services.GetRequiredService<IAvailabilityModuleApi>();
        _fixture = new Fixture();
    }

    public async Task InitializeAsync()
    {
        // Clear and seed the database
        await TestUtils.ClearDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetAvailableSlotsAsync_ShouldReturnOnlyUnreservedSlots()
    {
        // Arrange
        var reservedSlots = _fixture.Build<SlotEntity>()
            .With(s => s.IsReserved, true)
            .With(s => s.Time, DateTime.UtcNow.AddDays(1))
            .CreateMany(3); // Generate 3 reserved slots

        var unreservedSlots = _fixture.Build<SlotEntity>()
            .With(s => s.IsReserved, false)
            .With(s => s.Time, DateTime.UtcNow.AddDays(1))
            .CreateMany(3); // Generate 3 unreserved slots

        var slots = reservedSlots.Concat(unreservedSlots).ToList();

        await TestUtils.AddToDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slots);

        // Act
        var availableSlots = (await _slotsModuleApi.GetAvailableSlotsAsync()).ToList();

        // Assert
        availableSlots.Should().NotBeEmpty();
        availableSlots.Should().OnlyContain(slot => !slot.IsReserved);
    }

    [Fact]
    public async Task GetSlotByIdAsync_ShouldReturnCorrectSlot_WhenSlotExists()
    {
        // Arrange
        var slot = _fixture.Create<SlotEntity>();
        await TestUtils.AddToDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slot);

        // Act
        var result = await _slotsModuleApi.GetSlotByIdAsync(slot.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(slot.Id);
        result.Time.Should().Be(slot.Time);
        result.Cost.Should().Be(slot.Cost);
    }

    [Fact]
    public async Task GetSlotByIdAsync_ShouldThrowSlotNotFoundException_WhenSlotDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await _slotsModuleApi.Invoking(api => api.GetSlotByIdAsync(nonExistentId))
            .Should().ThrowAsync<SlotNotFoundException>();
    }
}
