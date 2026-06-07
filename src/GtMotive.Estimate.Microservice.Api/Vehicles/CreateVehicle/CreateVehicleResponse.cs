using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.CreateVehicle
{
    /// <summary>
    /// Response model returned after creating a fleet vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleResponse"/> class.
    /// </remarks>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="vin">The vehicle VIN.</param>
    public sealed class CreateVehicleResponse(Guid vehicleId, string vin)
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
    }
}
