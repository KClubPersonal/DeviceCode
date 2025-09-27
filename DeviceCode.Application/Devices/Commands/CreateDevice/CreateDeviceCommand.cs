using MediatR;

namespace DeviceCode.Application.Devices.Commands.CreateDevice;
public record CreateDeviceCommand(string Name, string Brand) : IRequest<Guid>;
