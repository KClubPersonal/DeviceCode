using DeviceCode.Application.Dtos;
using MediatR;

namespace DeviceCode.Application.Devices.Queries.GetDeviceById;
public record GetDeviceByIdQuery(Guid Id) : IRequest<DeviceDto?>;