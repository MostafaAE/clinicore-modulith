using AutoFixture;
using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using FluentAssertions;
using Moq;

namespace CliniCore.Tests.Bookings.Application.GetAvailableBookings;
public class GetAvailableBookingsHandlerTests
{
    private readonly Mock<IAvailabilityModuleApi> _mockAvailabilityModuleApi;
    private readonly GetAvailableBookingsHandler _handler;
    private readonly IFixture _fixture;

    public GetAvailableBookingsHandlerTests()
    {
        _mockAvailabilityModuleApi = new Mock<IAvailabilityModuleApi>();
        _handler = new GetAvailableBookingsHandler(_mockAvailabilityModuleApi.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedAvailableBookings()
    {
        // Arrange
        var slots = _fixture.Build<SlotDto>()
                            .With(s => s.IsReserved, false)
                            .CreateMany(5);

        _mockAvailabilityModuleApi
            .Setup(api => api.GetAvailableSlotsAsync())
            .ReturnsAsync(slots);

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().BeEquivalentTo(slots, options => options
            .ExcludingMissingMembers());

        _mockAvailabilityModuleApi.Verify(api => api.GetAvailableSlotsAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAvailableSlots()
    {
        // Arrange
        _mockAvailabilityModuleApi
            .Setup(api => api.GetAvailableSlotsAsync())
            .ReturnsAsync(Enumerable.Empty<SlotDto>());

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().BeEmpty();
        _mockAvailabilityModuleApi.Verify(api => api.GetAvailableSlotsAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenAvailabilityApiThrows()
    {
        // Arrange
        _mockAvailabilityModuleApi
            .Setup(api => api.GetAvailableSlotsAsync())
            .ThrowsAsync(new Exception("API Error"));

        // Act
        Func<Task> action = async () => await _handler.Handle();

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("API Error");
        _mockAvailabilityModuleApi.Verify(api => api.GetAvailableSlotsAsync(), Times.Once);
    }
}
