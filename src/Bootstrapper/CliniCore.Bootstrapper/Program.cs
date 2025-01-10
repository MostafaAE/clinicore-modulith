using CliniCore.Modules.Appointments.Shell;
using CliniCore.Modules.Availability.Api;
using CliniCore.Modules.Bookings.Api;
using CliniCore.Modules.Confirmations.Api;
using CliniCore.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAvailabiliyModule(builder.Configuration)
    .AddBookingsModule()
    .AddAppointmentsModule(builder.Configuration)
    .AddConfirmationsModule()
    .AddSharedFramework(builder.Configuration);

var app = builder.Build();


app.UseSharedFramework();
app.UseBookingsModule();
app.UseAppointmentsModule();
app.UseConfirmationsModule();
app.UseAvailabiliyModule();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }