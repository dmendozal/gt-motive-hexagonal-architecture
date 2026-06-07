using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.CreateVehicle
{
    /// <summary>
    /// Request model for creating a fleet vehicle.
    /// </summary>
    public sealed class CreateVehicleRequest
    {
        /// <summary>
        /// Gets or sets the vehicle VIN.
        /// </summary>
        [Required]
        public string Vin { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand.
        /// </summary>
        [Required]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the vehicle model.
        /// </summary>
        [Required]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the manufacturing date.
        /// </summary>
        [Required]
        [JsonRequired]
        public DateOnly ManufacturingDate { get; set; }
    }
}
