using MediatR;

namespace DeviceCode.Application.Devices.Commands.DeleteDevice;
public record DeleteDeviceCommand(Guid Id) : IRequest;
