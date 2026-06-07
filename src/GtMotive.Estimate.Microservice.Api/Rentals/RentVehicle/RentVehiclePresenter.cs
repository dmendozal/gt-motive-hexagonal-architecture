using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Rentals.RentVehicle
{
    /// <summary>
    /// Presenter for the rent vehicle use case.
    /// </summary>
    public sealed class RentVehiclePresenter : IWebApiPresenter, IRentVehicleOutputPort
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(RentVehicleOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var rentVehicleResponse = new RentVehicleResponse(
                response.RentalId,
                response.VehicleId,
                response.PersonDocumentId,
                response.RentedAt);

            ActionResult = new CreatedResult($"/rentals/{rentVehicleResponse.RentalId}", rentVehicleResponse);
        }
    }
}
