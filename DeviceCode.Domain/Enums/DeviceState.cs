namespace DeviceCode.Domain.Enums;

/// <summary>
/// Represents the states of a device.
/// </summary>
public enum DeviceState
{
    /// <summary>
    /// The device is available for use.
    /// </summary>
    Available,

    /// <summary>
    /// The device is currently in use..
    /// </summary>
    InUse,

    /// <summary>
    /// The device is inactive.
    /// </summary>
    Inactive
}