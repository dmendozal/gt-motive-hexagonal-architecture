using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles;
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
    /// <param name="getAvailableVehiclesUseCase">The get available vehicles use case.</param>
    /// <param name="presenter">The get available vehicles presenter.</param>
    [ApiController]
    [Route("vehicles/available")]
    public sealed class GetAvailableVehiclesController(
        GetAvailableVehiclesUseCase getAvailableVehiclesUseCase,
        GetAvailableVehiclesPresenter presenter) : ControllerBase
    {
        /// <summary>
        /// Gets available fleet vehicles.
        /// </summary>
        /// <returns>The HTTP action result.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetAvailableVehiclesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            await getAvailableVehiclesUseCase.Execute(new GetAvailableVehiclesInput());
            return presenter.ActionResult;
        }
    }
}
