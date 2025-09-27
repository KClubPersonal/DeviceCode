using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Enums;

namespace DeviceCode.Domain.Interfaces;

/// <summary>
/// Defines the contract for persistence operations related to the Device entity.
/// </summary>
public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id);
    Task<IEnumerable<Device>> GetAllAsync();
    Task<IEnumerable<Device>> GetByBrandAsync(string brand);
    Task<IEnumerable<Device>> GetByStateAsync(DeviceState state);
    Task AddAsync(Device device);
    Task UpdateAsync(Device device);
    Task DeleteAsync(Guid id);
}