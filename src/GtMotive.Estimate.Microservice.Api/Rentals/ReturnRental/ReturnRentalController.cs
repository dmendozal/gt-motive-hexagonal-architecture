using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental
{
    /// <summary>
    /// Returns rented fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnRentalController"/> class.
    /// </remarks>
    /// <param name="mediator">The mediator.</param>
    [ApiController]
    [Route("rentals/{rentalId:guid}/return")]
    public sealed class ReturnRentalController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Returns a rented fleet vehicle.
        /// </summary>
        /// <param name="rentalId">The rental identifier.</param>
        /// <param name="request">The request model.</param>
        /// <returns>The HTTP action result.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ReturnRentalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(Guid rentalId, ReturnRentalRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            request.RentalId = rentalId;
            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }
    }
}
