using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    public sealed class CreateVehicleEndpointTests(GenericInfrastructureTestServerFixture fixture)
        : InfrastructureTestBase(fixture)
    {
        [Fact]
        public async Task PostVehiclesWhenVinIsMissingReturnsBadRequest()
        {
            using var client = Fixture.Server.CreateClient();
            using var content = new StringContent(
                "{\"brand\":\"Toyota\",\"model\":\"Corolla\",\"manufacturingDate\":\"2023-01-10\"}",
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), content);
            var responseBody = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().Contain("The Vin field is required.");
        }

        [Fact]
        public async Task GetAvailableVehiclesWhenVehicleWasCreatedReturnsOkWithVehicle()
        {
            using var client = Fixture.Server.CreateClient();
            var vin = $"VIN-{Guid.NewGuid():N}";
            using var createVehicleContent = new StringContent(
                $"{{\"vin\":\"{vin}\",\"brand\":\"Toyota\",\"model\":\"Corolla\",\"manufacturingDate\":\"2023-01-10\"}}",
                Encoding.UTF8,
                "application/json");

            var createVehicleResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), createVehicleContent);
            var getAvailableVehiclesResponse = await client.GetAsync(new Uri("/vehicles/available", UriKind.Relative));
            var responseBody = await getAvailableVehiclesResponse.Content.ReadAsStringAsync();

            createVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            getAvailableVehiclesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody.Should().Contain(vin);
            responseBody.Should().Contain("Toyota");
            responseBody.Should().Contain("Corolla");
        }
    }
}
