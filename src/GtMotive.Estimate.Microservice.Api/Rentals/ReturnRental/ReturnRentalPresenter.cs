using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental
{
    /// <summary>
    /// Presenter for the return rental use case.
    /// </summary>
    public sealed class ReturnRentalPresenter : IWebApiPresenter, IReturnRentalOutputPort
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(ReturnRentalOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            ActionResult = new OkObjectResult(new ReturnRentalResponse(
                response.RentalId,
                response.VehicleId,
                response.ReturnedAt));
        }
    }
}
