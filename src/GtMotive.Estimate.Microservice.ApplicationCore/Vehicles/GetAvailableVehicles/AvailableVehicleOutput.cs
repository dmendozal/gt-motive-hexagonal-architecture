using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Available vehicle output data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AvailableVehicleOutput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="vin">The vehicle VIN.</param>
    /// <param name="brand">The vehicle brand.</param>
    /// <param name="model">The vehicle model.</param>
    /// <param name="manufacturingDate">The manufacturing date.</param>
    public sealed class AvailableVehicleOutput(Guid vehicleId, string vin, string brand, string model, DateOnly manufacturingDate)
    {
        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the vehicle VIN.
        /// </summary>
        public string Vin { get; } = vin;

        /// <summary>
        /// Gets the vehicle brand.
        /// </summary>
        public string Brand { get; } = brand;

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        public string Model { get; } = model;

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly ManufacturingDate { get; } = manufacturingDate;
    }
}
