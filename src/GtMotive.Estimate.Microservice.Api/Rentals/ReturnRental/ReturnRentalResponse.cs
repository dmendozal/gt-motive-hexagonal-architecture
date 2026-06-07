using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental
{
    /// <summary>
    /// Response model returned after returning a rental.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnRentalResponse"/> class.
    /// </remarks>
    /// <param name="rentalId">The rental identifier.</param>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="returnedAt">The return timestamp.</param>
    public sealed class ReturnRentalResponse(Guid rentalId, Guid vehicleId, DateTime returnedAt)
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
        /// Gets the return timestamp.
        /// </summary>
        [Required]
        public DateTime ReturnedAt { get; } = returnedAt;
    }
}
