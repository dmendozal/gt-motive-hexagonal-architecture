using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Rentals.RentVehicle
{
    /// <summary>
    /// Response model returned after renting a vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleResponse"/> class.
    /// </remarks>
    /// <param name="rentalId">The rental identifier.</param>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="personDocumentId">The person document identifier.</param>
    /// <param name="rentedAt">The rent timestamp.</param>
    public sealed class RentVehicleResponse(Guid rentalId, Guid vehicleId, string personDocumentId, DateTime rentedAt)
    {
        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        [Required]
        public Guid RentalId { get; } = rentalId;

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        [Required]
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the person document identifier.
        /// </summary>
        [Required]
        public string PersonDocumentId { get; } = personDocumentId;

        /// <summary>
        /// Gets the rent timestamp.
        /// </summary>
        [Required]
        public DateTime RentedAt { get; } = rentedAt;
    }
}
