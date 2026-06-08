using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Gets available fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesController"/> class.
    /// </remarks>
    /// <param name="mediator">The mediator.</param>
    [ApiController]
    [Route("vehicles/available")]
    public sealed class GetAvailableVehiclesController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets available fleet vehicles.
        /// </summary>
        /// <returns>The HTTP action result.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetAvailableVehiclesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var presenter = await mediator.Send(new GetAvailableVehiclesRequest());
            return presenter.ActionResult;
        }
    }
}
