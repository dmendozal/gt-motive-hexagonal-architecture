using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    public sealed class RentVehicleEndpointTests(GenericInfrastructureTestServerFixture fixture)
        : InfrastructureTestBase(fixture)
    {
        [Fact]
        public async Task PostRentalsWhenVehicleIsAvailableReturnsCreated()
        {
            using var client = Fixture.Server.CreateClient();
            var vin = $"VIN-{Guid.NewGuid():N}";
            using var createVehicleContent = new StringContent(
                $"{{\"vin\":\"{vin}\",\"brand\":\"Toyota\",\"model\":\"Corolla\",\"manufacturingDate\":\"2023-01-10\"}}",
                Encoding.UTF8,
                "application/json");
            var createVehicleResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), createVehicleContent);
            var createVehicleBody = await createVehicleResponse.Content.ReadAsStringAsync();
            var vehicleId = JsonDocument.Parse(createVehicleBody).RootElement.GetProperty("vehicleId").GetGuid();
            using var rentVehicleContent = new StringContent(
                $"{{\"vehicleId\":\"{vehicleId}\",\"personDocumentId\":\"12345678A\",\"personName\":\"Jane Doe\"}}",
                Encoding.UTF8,
                "application/json");

            var rentVehicleResponse = await client.PostAsync(new Uri("/rentals", UriKind.Relative), rentVehicleContent);
            var rentVehicleBody = await rentVehicleResponse.Content.ReadAsStringAsync();

            createVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            rentVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            rentVehicleBody.Should().Contain(vehicleId.ToString());
            rentVehicleBody.Should().Contain("12345678A");
        }

        [Fact]
        public async Task PostReturnRentalWhenRentalIsActiveReturnsOkAndVehicleBecomesAvailable()
        {
            using var client = Fixture.Server.CreateClient();
            var vin = $"VIN-{Guid.NewGuid():N}";
            var personDocumentId = $"DOC-{Guid.NewGuid():N}";
            using var createVehicleContent = new StringContent(
                $"{{\"vin\":\"{vin}\",\"brand\":\"Toyota\",\"model\":\"Corolla\",\"manufacturingDate\":\"2023-01-10\"}}",
                Encoding.UTF8,
                "application/json");
            var createVehicleResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), createVehicleContent);
            var createVehicleBody = await createVehicleResponse.Content.ReadAsStringAsync();
            var vehicleId = JsonDocument.Parse(createVehicleBody).RootElement.GetProperty("vehicleId").GetGuid();
            using var rentVehicleContent = new StringContent(
                $"{{\"vehicleId\":\"{vehicleId}\",\"personDocumentId\":\"{personDocumentId}\",\"personName\":\"Jane Doe\"}}",
                Encoding.UTF8,
                "application/json");
            var rentVehicleResponse = await client.PostAsync(new Uri("/rentals", UriKind.Relative), rentVehicleContent);
            var rentVehicleBody = await rentVehicleResponse.Content.ReadAsStringAsync();
            var rentalId = JsonDocument.Parse(rentVehicleBody).RootElement.GetProperty("rentalId").GetGuid();
            using var returnRentalContent = new StringContent(
                $"{{\"personDocumentId\":\"{personDocumentId}\"}}",
                Encoding.UTF8,
                "application/json");

            var returnRentalResponse = await client.PostAsync(
                new Uri($"/rentals/{rentalId}/return", UriKind.Relative),
                returnRentalContent);
            var getAvailableVehiclesResponse = await client.GetAsync(new Uri("/vehicles/available", UriKind.Relative));
            var availableVehiclesBody = await getAvailableVehiclesResponse.Content.ReadAsStringAsync();

            createVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            rentVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            returnRentalResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getAvailableVehiclesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            availableVehiclesBody.Should().Contain(vin);
        }
    }
}
