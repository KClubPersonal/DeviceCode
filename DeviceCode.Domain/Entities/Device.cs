using System.Text.Json.Serialization;
using DeviceCode.Domain.Enums;
using DeviceCode.Domain.Exceptions;

namespace DeviceCode.Domain.Entities;

/// <summary>
/// Represents the core entity of the domain, the Device.
/// It encapsulates the properties and business rules associated with a device.
/// </summary>
public class Device
{
    /// <summary>
    /// The unique identifier.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The name of the device.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// The brand of the device.
    /// </summary>
    public string Brand { get; private set; } = string.Empty;

    /// <summary>
    /// The current state of the device.
    /// </summary>
    public DeviceState State { get; private set; }

    /// <summary>
    /// The datetime when the device was created. This property cannot be changed after creation.
    /// </summary>
    public DateTime CreationTime { get; private set; }

    /// <summary>
    /// I used this constructor to be used by the MongoDB Driver (DeviceMap) to create an object while still ensuring that it will respect the Domain rules.
    /// An empty constructor could be used, but that would take away the Domain's responsibility for ensuring integrity.
    /// </summary>
    private Device(Guid id, string name, string brand, DeviceState state, DateTime creationTime)
    {
        Id = id;
        Name = name;
        Brand = brand;
        State = state;
        CreationTime = creationTime;
    }

    /// <summary>
    /// Factory method to create a new device, ensuring its initial state is valid.
    /// </summary>
    public static Device Create(string name, string brand)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Device name is required.");
        }

        if (string.IsNullOrWhiteSpace(brand))
        {
            throw new DomainException("Device brand is required.");
        }

        var newId = Guid.NewGuid();
        var initialState = DeviceState.Available;
        var creationTimestamp = DateTime.UtcNow;

        return new Device(newId, name, brand, initialState, creationTimestamp);

    }

    /// <summary>
    /// Updates the name and brand of the device.
    /// </summary>
    /// <param name="name">The new name for the device.</param>
    /// <param name="brand">The new brand for the device.</param>
    /// <exception cref="DomainException">Thrown when attempting to update a device that is in use.</exception>
    public void UpdateDetails(string name, string brand)
    {
        if (State == DeviceState.InUse)
        {
            throw new DomainException("Cannot update name and brand for a device that is in use.");
        }

        Name = name;
        Brand = brand;
    }

    /// <summary>
    /// Updates the state of the device.
    /// </summary>
    /// <param name="newState">The new state for the device.</param>
    public void SetState(DeviceState newState)
    {
        State = newState;
    }
}