using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.Rentals.RentVehicle
{
    /// <summary>
    /// Request model for renting a vehicle.
    /// </summary>
    public sealed class RentVehicleRequest
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        [Required]
        [JsonRequired]
        public Guid VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the person document identifier.
        /// </summary>
        [Required]
        public string PersonDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the person name.
        /// </summary>
        [Required]
        public string PersonName { get; set; }
    }
}
