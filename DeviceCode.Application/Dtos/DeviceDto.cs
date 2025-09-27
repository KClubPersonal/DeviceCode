using DeviceCode.Domain.Enums;

namespace DeviceCode.Application.Dtos;


/// <summary>
/// DTO to be used from API to Application
/// </summary>
public record DeviceDto(
    Guid Id,
    string Name,
    string Brand,
    DeviceState State,
    DateTime CreationTime
);