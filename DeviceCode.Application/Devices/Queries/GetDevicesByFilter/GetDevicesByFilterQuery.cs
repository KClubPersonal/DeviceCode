using DeviceCode.Application.Dtos;
using DeviceCode.Domain.Enums;
using MediatR;

namespace DeviceCode.Application.Devices.Queries.GetDevicesByFilter;
public record GetDevicesByFilterQuery(string? Brand, DeviceState? State)    : IRequest<IEnumerable<DeviceDto>>;
