using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Response model returned after getting available fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesResponse"/> class.
    /// </remarks>
    /// <param name="vehicles">The available vehicles.</param>
    public sealed class GetAvailableVehiclesResponse(IReadOnlyCollection<AvailableVehicleResponse> vehicles)
    {
        /// <summary>
        /// Gets the available vehicles.
        /// </summary>
        [Required]
        public IReadOnlyCollection<AvailableVehicleResponse> Vehicles { get; } = vehicles;
    }
}
