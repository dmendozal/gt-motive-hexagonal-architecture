using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Available vehicle response model.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AvailableVehicleResponse"/> class.
    /// </remarks>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="vin">The vehicle VIN.</param>
    /// <param name="brand">The vehicle brand.</param>
    /// <param name="model">The vehicle model.</param>
    /// <param name="manufacturingDate">The manufacturing date.</param>
    public sealed class AvailableVehicleResponse(Guid vehicleId, string vin, string brand, string model, DateOnly manufacturingDate)
    {
        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        [Required]
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the vehicle VIN.
        /// </summary>
        [Required]
        public string Vin { get; } = vin;

        /// <summary>
        /// Gets the vehicle brand.
        /// </summary>
        [Required]
        public string Brand { get; } = brand;

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        [Required]
        public string Model { get; } = model;

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        [Required]
        public DateOnly ManufacturingDate { get; } = manufacturingDate;
    }
}
