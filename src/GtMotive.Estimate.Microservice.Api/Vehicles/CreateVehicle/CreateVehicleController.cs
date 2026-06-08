using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.CreateVehicle
{
    /// <summary>
    /// Creates fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleController"/> class.
    /// </remarks>
    /// <param name="mediator">The mediator.</param>
    [ApiController]
    [Route("vehicles")]
    public sealed class CreateVehicleController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a fleet vehicle.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <returns>The HTTP action result.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateVehicleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }
    }
}
