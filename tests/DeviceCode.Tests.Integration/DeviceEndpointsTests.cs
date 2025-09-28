using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeviceCode.Application.Dtos;
using DeviceCode.Domain.Enums;
using FluentAssertions;

namespace DeviceCode.Tests.Integration;
public class DeviceEndpointsTests(DeviceApiFactory factory) : IClassFixture<DeviceApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task PostDevice_WhenCreated_ReturnsCreatedAndCanBeFetched()
    {
        // Arrange
        var newDeviceRequest = new { Name = "Test Device", Brand = "Integration Test" };

        // Act (1)
        var postResponse = await _client.PostAsJsonAsync("/devices", newDeviceRequest);

        // Assert (1)
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdDevice = await postResponse.Content.ReadFromJsonAsync<CreatedDeviceResponse>();
        createdDevice.Should().NotBeNull();
        createdDevice.Id.Should().NotBeEmpty();

        // Act (2)
        var getResponse = await _client.GetAsync($"/devices/{createdDevice.Id}");

        // Assert (2)
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetchedDevice = await getResponse.Content.ReadFromJsonAsync<DeviceDto>(JsonOpts);
        fetchedDevice.Should().NotBeNull();
        fetchedDevice.Name.Should().Be(newDeviceRequest.Name);
        fetchedDevice.Brand.Should().Be(newDeviceRequest.Brand);
    }

    [Fact]
    public async Task PutDevice_WhenDeviceExists_ShouldUpdateDeviceAndReturnNoContent()
    {
        // Arrange
        var createRequest = new { Name = "Old Name", Brand = "Old Brand" };
        var postResponse = await _client.PostAsJsonAsync("/devices", createRequest);
        var createdDevice = await postResponse.Content.ReadFromJsonAsync<CreatedDeviceResponse>();
        var deviceId = createdDevice!.Id;

        var updateRequest = new { Name = "New Name", Brand = "New Brand", State = "Inactive" };

        // Act
        var putResponse = await _client.PutAsJsonAsync($"/devices/{deviceId}", updateRequest);

        // Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetFromJsonAsync<DeviceDto>($"/devices/{deviceId}", JsonOpts);

        getResponse.Should().NotBeNull();
        getResponse.Name.Should().Be("New Name");
        getResponse.State.Should().Be(DeviceState.Inactive);
    }

    [Fact]
    public async Task PutDevice_WhenUpdatingNameOfInUseDevice_ShouldReturnBadRequest()
    {
        // Arrange
        var createRequest = new { Name = "iPhone 17", Brand = "Apple" };
        var postResponse = await _client.PostAsJsonAsync("/devices", createRequest);
        var createdDevice = await postResponse.Content.ReadFromJsonAsync<CreatedDeviceResponse>(JsonOpts);
        var deviceId = createdDevice!.Id;

        var setInUseRequest = new { Name = "iPhone 17", Brand = "Apple", State = "InUse" };
        await _client.PutAsJsonAsync($"/devices/{deviceId}", setInUseRequest);

        var invalidUpdateRequest = new { Name = "iPhone 17 Pro Max", Brand = "Apple", State = "InUse" };

        // Act
        var putResponse = await _client.PutAsJsonAsync($"/devices/{deviceId}", invalidUpdateRequest);

        // Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // To Post
    private record CreatedDeviceResponse(Guid Id);
}
