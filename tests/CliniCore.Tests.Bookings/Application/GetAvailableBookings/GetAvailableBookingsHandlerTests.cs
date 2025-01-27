using AutoFixture;
using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using CliniCore.Modules.Bookings.Application.Interfaces;
using FluentAssertions;
using Moq;

namespace CliniCore.Tests.Bookings.Application.GetAvailableBookings;
public class GetAvailableBookingsHandlerTests
{
    private readonly Mock<IAvailabilityService> _mockAvailabilityServiceMock;
    private readonly GetAvailableBookingsHandler _handler;
    private readonly IFixture _fixture;

    public GetAvailableBookingsHandlerTests()
    {
        _mockAvailabilityServiceMock = new Mock<IAvailabilityService>();
        _handler = new GetAvailableBookingsHandler(_mockAvailabilityServiceMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedAvailableBookings()
    {
        // Arrange
        var slots = _fixture.Build<AvailableBookingDto>()
                            .With(s => s.IsReserved, false)
                            .CreateMany(5);

        _mockAvailabilityServiceMock
            .Setup(api => api.GetAvailableSlotsAsync())
            .ReturnsAsync(slots);

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().BeEquivalentTo(slots, options => options
            .ExcludingMissingMembers());

        _mockAvailabilityServiceMock.Verify(api => api.GetAvailableSlotsAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAvailableSlots()
    {
        // Arrange
        _mockAvailabilityServiceMock
            .Setup(api => api.GetAvailableSlotsAsync())
            .ReturnsAsync(Enumerable.Empty<AvailableBookingDto>());

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().BeEmpty();
        _mockAvailabilityServiceMock.Verify(api => api.GetAvailableSlotsAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenAvailabilityApiThrows()
    {
        // Arrange
        _mockAvailabilityServiceMock
            .Setup(api => api.GetAvailableSlotsAsync())
            .ThrowsAsync(new Exception("API Error"));

        // Act
        Func<Task> action = async () => await _handler.Handle();

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("API Error");
        _mockAvailabilityServiceMock.Verify(api => api.GetAvailableSlotsAsync(), Times.Once);
    }
}
