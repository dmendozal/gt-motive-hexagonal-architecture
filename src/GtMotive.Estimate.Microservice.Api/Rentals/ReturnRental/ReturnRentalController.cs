using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Common;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental;
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
    /// <param name="returnRentalUseCase">The return rental use case.</param>
    /// <param name="presenter">The return rental presenter.</param>
    [ApiController]
    [Route("rentals/{rentalId:guid}/return")]
    public sealed class ReturnRentalController(ReturnRentalUseCase returnRentalUseCase, ReturnRentalPresenter presenter) : ControllerBase
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

            await returnRentalUseCase.Execute(new ReturnRentalInput(rentalId, request.PersonDocumentId));
            return presenter.ActionResult;
        }
    }
}
