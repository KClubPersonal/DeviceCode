using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeviceCode.Application.Dtos;
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

    // To Post
    private record CreatedDeviceResponse(Guid Id);
}
