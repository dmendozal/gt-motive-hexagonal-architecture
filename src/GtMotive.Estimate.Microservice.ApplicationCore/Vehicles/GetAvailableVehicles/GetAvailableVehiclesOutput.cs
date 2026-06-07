using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Output data returned after getting available fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesOutput"/> class.
    /// </remarks>
    /// <param name="vehicles">The available vehicles.</param>
    public sealed class GetAvailableVehiclesOutput
        (IReadOnlyCollection<AvailableVehicleOutput> vehicles) : IUseCaseOutput
    {
        /// <summary>
        /// Gets the available vehicles.
        /// </summary>
        public IReadOnlyCollection<AvailableVehicleOutput> Vehicles { get; } = vehicles;
    }
}
