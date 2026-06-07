using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.CreateVehicle
{
    /// <summary>
    /// Presenter for the creation vehicle use case.
    /// </summary>
    public sealed class CreateVehiclePresenter : IWebApiPresenter, ICreateVehicleOutputPort
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(CreateVehicleOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var createVehicleResponse = new CreateVehicleResponse(response.VehicleId, response.Vin);
            ActionResult = new CreatedResult($"/vehicles/{createVehicleResponse.VehicleId}", createVehicleResponse);
        }

        /// <inheritdoc />
        public void ConflictHandle(string message)
        {
            ArgumentNullException.ThrowIfNull(message);

            ActionResult = new ConflictObjectResult(new { detail = message });
        }
    }
}
