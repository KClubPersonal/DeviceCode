using DeviceCode.Domain.Enums;
using MediatR;

namespace DeviceCode.Application.Devices.Commands.UpdateDevice;
public record UpdateDeviceCommand(Guid Id, string Name, string Brand, DeviceState State) : IRequest;