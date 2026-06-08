using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Rentals.RentVehicle
{
    /// <summary>
    /// Rents fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleController"/> class.
    /// </remarks>
    /// <param name="mediator">The mediator.</param>
    [ApiController]
    [Route("rentals")]
    public sealed class RentVehicleController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Rents a fleet vehicle.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <returns>The HTTP action result.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RentVehicleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(RentVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }
    }
}
