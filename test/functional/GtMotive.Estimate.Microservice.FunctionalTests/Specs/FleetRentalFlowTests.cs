using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    public sealed class FleetRentalFlowTests(CompositionRootTestFixture fixture)
        : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task FullFleetRentalFlowWorksEndToEnd()
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
            using var createVehicleJson = JsonDocument.Parse(createVehicleBody);
            var vehicleId = createVehicleJson.RootElement.GetProperty("vehicleId").GetGuid();

            var availableBeforeRent = await client.GetAsync(new Uri("/vehicles/available", UriKind.Relative));
            var availableBeforeRentBody = await availableBeforeRent.Content.ReadAsStringAsync();

            using var rentVehicleContent = new StringContent(
                $"{{\"vehicleId\":\"{vehicleId}\",\"personDocumentId\":\"{personDocumentId}\",\"personName\":\"Jane Doe\"}}",
                Encoding.UTF8,
                "application/json");

            var rentVehicleResponse = await client.PostAsync(new Uri("/rentals", UriKind.Relative), rentVehicleContent);
            var rentVehicleBody = await rentVehicleResponse.Content.ReadAsStringAsync();
            using var rentVehicleJson = JsonDocument.Parse(rentVehicleBody);
            var rentalId = rentVehicleJson.RootElement.GetProperty("rentalId").GetGuid();

            var availableAfterRent = await client.GetAsync(new Uri("/vehicles/available", UriKind.Relative));
            var availableAfterRentBody = await availableAfterRent.Content.ReadAsStringAsync();

            using var returnRentalContent = new StringContent(
                $"{{\"personDocumentId\":\"{personDocumentId}\"}}",
                Encoding.UTF8,
                "application/json");

            var returnRentalResponse = await client.PostAsync(
                new Uri($"/rentals/{rentalId}/return", UriKind.Relative),
                returnRentalContent);

            var availableAfterReturn = await client.GetAsync(new Uri("/vehicles/available", UriKind.Relative));
            var availableAfterReturnBody = await availableAfterReturn.Content.ReadAsStringAsync();

            createVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createVehicleBody.Should().Contain(vin);
            availableBeforeRent.StatusCode.Should().Be(HttpStatusCode.OK);
            availableBeforeRentBody.Should().Contain(vin);
            rentVehicleResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            rentVehicleBody.Should().Contain(personDocumentId);
            availableAfterRent.StatusCode.Should().Be(HttpStatusCode.OK);
            availableAfterRentBody.Should().NotContain(vin);
            returnRentalResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            availableAfterReturn.StatusCode.Should().Be(HttpStatusCode.OK);
            availableAfterReturnBody.Should().Contain(vin);
        }

        [Fact]
        public async Task PostVehiclesWhenVinAlreadyExistsReturnsConflict()
        {
            using var client = Fixture.Server.CreateClient();
            var vin = $"VIN-{Guid.NewGuid():N}";

            using var createVehicleContent = new StringContent(
                $"{{\"vin\":\"{vin}\",\"brand\":\"Toyota\",\"model\":\"Corolla\",\"manufacturingDate\":\"2023-01-10\"}}",
                Encoding.UTF8,
                "application/json");

            var firstResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), createVehicleContent);

            using var duplicateVehicleContent = new StringContent(
                $"{{\"vin\":\"{vin}\",\"brand\":\"Ford\",\"model\":\"Focus\",\"manufacturingDate\":\"2024-02-20\"}}",
                Encoding.UTF8,
                "application/json");

            var duplicateResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), duplicateVehicleContent);
            var duplicateBody = await duplicateResponse.Content.ReadAsStringAsync();

            firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            duplicateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
            duplicateBody.Should().Contain("Vehicle already exists.");
        }

        [Fact]
        public async Task PostRentalsWhenVehicleDoesNotExistReturnsNotFound()
        {
            using var client = Fixture.Server.CreateClient();
            var requestBody = $"{{\"vehicleId\":\"{Guid.NewGuid()}\",\"personDocumentId\":\"DOC-{Guid.NewGuid():N}\",\"personName\":\"Jane Doe\"}}";
            using var rentContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(new Uri("/rentals", UriKind.Relative), rentContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody.Should().Contain("Vehicle was not found.");
        }

        [Fact]
        public async Task PostReturnRentalWhenRentalDoesNotExistReturnsNotFound()
        {
            using var client = Fixture.Server.CreateClient();
            using var returnContent = new StringContent(
                $"{{\"personDocumentId\":\"DOC-{Guid.NewGuid():N}\"}}",
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                new Uri($"/rentals/{Guid.NewGuid()}/return", UriKind.Relative),
                returnContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody.Should().Contain("Rental not found.");
        }

        [Fact]
        public async Task PostRentalsWhenPersonAlreadyHasActiveRentalReturnsBadRequest()
        {
            using var client = Fixture.Server.CreateClient();
            var vinOne = $"VIN-{Guid.NewGuid():N}";
            var vinTwo = $"VIN-{Guid.NewGuid():N}";
            var personDocumentId = $"DOC-{Guid.NewGuid():N}";

            using var createVehicleOneContent = new StringContent(
                $"{{\"vin\":\"{vinOne}\",\"brand\":\"Toyota\",\"model\":\"Corolla\",\"manufacturingDate\":\"2023-01-10\"}}",
                Encoding.UTF8,
                "application/json");
            using var createVehicleTwoContent = new StringContent(
                $"{{\"vin\":\"{vinTwo}\",\"brand\":\"Ford\",\"model\":\"Focus\",\"manufacturingDate\":\"2023-01-10\"}}",
                Encoding.UTF8,
                "application/json");

            var firstVehicleResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), createVehicleOneContent);
            var firstVehicleBody = await firstVehicleResponse.Content.ReadAsStringAsync();
            using var firstVehicleJson = JsonDocument.Parse(firstVehicleBody);
            var firstVehicleId = firstVehicleJson.RootElement.GetProperty("vehicleId").GetGuid();

            var secondVehicleResponse = await client.PostAsync(new Uri("/vehicles", UriKind.Relative), createVehicleTwoContent);
            var secondVehicleBody = await secondVehicleResponse.Content.ReadAsStringAsync();
            using var secondVehicleJson = JsonDocument.Parse(secondVehicleBody);
            var secondVehicleId = secondVehicleJson.RootElement.GetProperty("vehicleId").GetGuid();

            using var firstRentContent = new StringContent(
                $"{{\"vehicleId\":\"{firstVehicleId}\",\"personDocumentId\":\"{personDocumentId}\",\"personName\":\"Jane Doe\"}}",
                Encoding.UTF8,
                "application/json");
            using var secondRentContent = new StringContent(
                $"{{\"vehicleId\":\"{secondVehicleId}\",\"personDocumentId\":\"{personDocumentId}\",\"personName\":\"Jane Doe\"}}",
                Encoding.UTF8,
                "application/json");

            var firstRentResponse = await client.PostAsync(new Uri("/rentals", UriKind.Relative), firstRentContent);
            var secondRentResponse = await client.PostAsync(new Uri("/rentals", UriKind.Relative), secondRentContent);
            var secondRentBody = await secondRentResponse.Content.ReadAsStringAsync();

            firstRentResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            secondRentResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            secondRentBody.Should().Contain("Person already has an active rental.");
        }
    }
}
