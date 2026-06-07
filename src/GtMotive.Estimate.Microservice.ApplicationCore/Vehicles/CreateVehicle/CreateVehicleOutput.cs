using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle
{
    /// <summary>
    /// Output data returned after creating a fleet vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleOutput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The created vehicle identifier.</param>
    /// <param name="vin">The created vehicle VIN.</param>
    public sealed class CreateVehicleOutput(Guid vehicleId, string vin) : IUseCaseOutput
    {
        /// <summary>
        /// Gets the created vehicle identifier.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the created vehicle VIN.
        /// </summary>
        public string Vin { get; } = vin;
    }
}
