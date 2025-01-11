using AutoFixture;
using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace CliniCore.Tests.Bookings.Api;
public class GetAvailableBookingsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;
    private readonly Mock<IAvailabilityModuleApi> _availabilityModuleApiMock;
    private readonly HttpClient _httpClient;


    public GetAvailableBookingsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _availabilityModuleApiMock = new Mock<IAvailabilityModuleApi>();
        _httpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => _availabilityModuleApiMock.Object);
            });
        }).CreateClient();
    }
    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetAvailableBookings_ShouldReturnOkWithAvailableBookings()
    {
        // Arrange
        var availableSlots = _fixture.CreateMany<SlotDto>(5).ToList();
        _availabilityModuleApiMock
            .Setup(api => api.GetAvailableSlotsAsync())
            .ReturnsAsync(availableSlots);

        // Act
        var response = await _httpClient.GetAsync("/api/v1/Bookings/Available");

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
        _availabilityModuleApiMock
            .Setup(api => api.GetAvailableSlotsAsync())
            .ThrowsAsync(new Exception("Service unavailable"));

        // Act
        var response = await _httpClient.GetAsync("/api/v1/Bookings/Available");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
