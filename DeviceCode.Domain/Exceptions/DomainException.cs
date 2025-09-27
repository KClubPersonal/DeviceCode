namespace DeviceCode.Domain.Exceptions;
/// <summary>
/// Custom exception Domain.
/// </summary>
public class DomainException(string message) : Exception(message)
{
}
