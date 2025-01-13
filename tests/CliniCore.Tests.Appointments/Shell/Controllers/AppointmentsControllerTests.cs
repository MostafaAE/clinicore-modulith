using AutoFixture;
using CliniCore.Modules.Appointments.Core.DTO;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Shell.DAL;
using CliniCore.Modules.Appointments.Shell.DAL.Entities;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace CliniCore.Tests.Appointments.Shell.Controllers;
public class AppointmentsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;
    private readonly HttpClient _client;

    public AppointmentsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory); // Clear the database before each test
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetUpcomingAppointments_ShouldReturnOnlyUpcomingAppointments_WhenAppointmentsExist()
    {
        // Arrange
        var upcomingAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(10))
            .With(a => a.Status, AppointmentStatus.Booked)
            .CreateMany(3);

        var upcomingCompletedAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(10))
            .With(a => a.Status, AppointmentStatus.Completed)
            .CreateMany(3);

        var upcomingCanceledAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(10))
            .With(a => a.Status, AppointmentStatus.Canceled)
            .CreateMany(3);

        var passedAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(-10))
            .With(a => a.Status, AppointmentStatus.Canceled)
            .CreateMany(3);

        var allAppointments = upcomingAppointments
            .Concat(upcomingCompletedAppointments)
            .Concat(upcomingCanceledAppointments)
            .Concat(passedAppointments);

        await TestUtils.AddToDatabaseAsync<AppointmentsDbContext, AppointmentEntity>(_factory, allAppointments);

        // Act
        var response = await _client.GetAsync("/api/v1/appointments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var appointments = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentDto>>();
        appointments.Should().NotBeNull();
        appointments.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetUpcomingAppointments_ShouldReturnEmptyList_WhenNoAppointmentsExist()
    {
        // Act
        var response = await _client.GetAsync("/appointments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var appointments = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentDto>>();
        appointments.Should().NotBeNull();
        appointments.Should().BeEmpty();
    }
}
