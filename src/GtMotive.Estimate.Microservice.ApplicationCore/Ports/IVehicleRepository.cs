using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Ports
{
    /// <summary>
    /// Vehicle persistence port.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Checks whether a vehicle exists for the specified VIN.
        /// </summary>
        /// <param name="vin">The vehicle VIN.</param>
        /// <returns>A value indicating whether the vehicle exists.</returns>
        Task<bool> ExistsByVin(Vin vin);

        /// <summary>
        /// Adds a vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Vehicle vehicle);

        /// <summary>
        /// Gets a vehicle by its identifier.
        /// </summary>
        /// <param name="vehicleId">The vehicle identifier.</param>
        /// <returns>The vehicle when found; otherwise null.</returns>
        Task<Vehicle> GetById(Guid vehicleId);

        /// <summary>
        /// Updates a vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(Vehicle vehicle);

        /// <summary>
        /// Gets all available vehicles.
        /// </summary>
        /// <returns>The available vehicles.</returns>
        Task<IReadOnlyCollection<Vehicle>> GetAvailable();
    }
}
