using System;
using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Presenter for the get available vehicles use case.
    /// </summary>
    public sealed class GetAvailableVehiclesPresenter : IWebApiPresenter, IGetAvailableVehiclesOutputPort
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(GetAvailableVehiclesOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var availableVehicles = new List<AvailableVehicleResponse>();
            foreach (var vehicle in response.Vehicles)
            {
                availableVehicles.Add(new AvailableVehicleResponse(
                    vehicle.VehicleId,
                    vehicle.Vin,
                    vehicle.Brand,
                    vehicle.Model,
                    vehicle.ManufacturingDate));
            }

            ActionResult = new OkObjectResult(new GetAvailableVehiclesResponse(availableVehicles));
        }
    }
}
