using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Common;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle;
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
    /// <param name="createVehicleUseCase">The create vehicle use case.</param>
    /// <param name="presenter">The create vehicle presenter.</param>
    [ApiController]
    [Route("vehicles")]
    public sealed class CreateVehicleController(CreateVehicleUseCase createVehicleUseCase, CreateVehiclePresenter presenter) : ControllerBase
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

            var input = new CreateVehicleInput(
                request.Vin,
                request.Brand,
                request.Model,
                request.ManufacturingDate);

            await createVehicleUseCase.Execute(input);
            return presenter.ActionResult;
        }
    }
}
