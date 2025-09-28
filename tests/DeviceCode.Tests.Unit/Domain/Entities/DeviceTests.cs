using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Enums;
using DeviceCode.Domain.Exceptions;
using FluentAssertions;

namespace DeviceCode.Tests.Unit.Domain.Entities;
public class DeviceTests
{
    [Fact]
    public void UpdateDetails_WhenDeviceIsInUse_ShouldThrowDomainException()
    {
        // Arrange 
        var device = Device.Create("Test Name", "Test Brand");                
        device.SetState(DeviceState.InUse);

        // Act 
        var action = () => device.UpdateDetails("New Name", "New Brand");

        // Assert 
        action.Should().Throw<DomainException>()
            .WithMessage("Cannot update name and brand*");
    }

    [Fact]
    public void UpdateDetails_WhenDeviceIsAvailable_ShouldUpdateProperties()
    {
        // Arrange
        var device = Device.Create("Old Name", "Old Brand");
        var newName = "New Name";
        var newBrand = "New Brand";

        // Act
        device.UpdateDetails(newName, newBrand);

        // Assert
        device.Name.Should().Be(newName);
        device.Brand.Should().Be(newBrand);
    }
}