using AutoFixture;
using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using CliniCore.Tests.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace CliniCore.Tests.Bookings.Api;
public class GetAvailableBookingsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;

    public GetAvailableBookingsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAvailableBookings_ShouldReturnOkWithAvailableBookings()
    {
        // Arrange
        var availableSlots = _fixture.CreateMany<SlotDto>(5).ToList();
        var mockAvailabilityApi = new Mock<IAvailabilityModuleApi>();
        mockAvailabilityApi
            .Setup(api => api.GetAvailableSlotsAsync())
            .ReturnsAsync(availableSlots);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => mockAvailabilityApi.Object);
            });
        }).CreateClient();


        // Act
        var response = await client.GetAsync("/api/v1/AvailableBookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseData = await response.Content.ReadFromJsonAsync<List<AvailableBookingDto>>();
        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(availableSlots);
    }

    [Fact]
    public async Task GetAvailableBookings_ShouldReturnInternalServerError_WhenExternalCallFails()
    {
        // Arrange
        var mockAvailabilityApi = new Mock<IAvailabilityModuleApi>();
        mockAvailabilityApi
            .Setup(api => api.GetAvailableSlotsAsync())
            .ThrowsAsync(new Exception("Service unavailable"));

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => mockAvailabilityApi.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/AvailableBookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
