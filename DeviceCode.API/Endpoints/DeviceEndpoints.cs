using DeviceCode.Application.Devices.Commands.CreateDevice;
using DeviceCode.Application.Devices.Commands.DeleteDevice;
using DeviceCode.Application.Devices.Commands.UpdateDevice;
using DeviceCode.Application.Devices.Queries.GetDeviceById;
using DeviceCode.Application.Devices.Queries.GetDevicesByFilter;
using DeviceCode.Domain.Enums;
using DeviceCode.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DeviceCode.API.Endpoints;

public record CreateDeviceRequest(string Name, string Brand);
public record UpdateDeviceRequest(string Name, string Brand, DeviceState State);

public static class DeviceEndpoints
{
    public static WebApplication MapDeviceEndpoints(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature?.Error is DomainException domainException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = domainException.Message });
                }
            });
        });

        var group = app.MapGroup("/devices");

        // POST /devices
        group.MapPost("/", async (CreateDeviceRequest request, IMediator mediator) =>
        {
            var command = new CreateDeviceCommand(request.Name, request.Brand);
            var deviceId = await mediator.Send(command);
            return Results.Created($"/devices/{deviceId}", new { id = deviceId });
        });

        // GET /devices?brand=...&state=...
        group.MapGet("/", async ([FromQuery] string? brand, [FromQuery] DeviceState? state, IMediator mediator) =>
        {
            var query = new GetDevicesByFilterQuery(brand, state);
            var devices = await mediator.Send(query);
            return Results.Ok(devices);
        });

        // GET /devices/{id}
        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var query = new GetDeviceByIdQuery(id);
            var device = await mediator.Send(query);
            return device is not null ? Results.Ok(device) : Results.NotFound();
        });

        // PUT /devices/{id}
        group.MapPut("/{id:guid}", async (Guid id, UpdateDeviceRequest request, IMediator mediator) =>
        {
            var command = new UpdateDeviceCommand(id, request.Name, request.Brand, request.State);
            await mediator.Send(command);
            return Results.NoContent();
        });

        // DELETE /devices/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var command = new DeleteDeviceCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });

        return app;
    }
}