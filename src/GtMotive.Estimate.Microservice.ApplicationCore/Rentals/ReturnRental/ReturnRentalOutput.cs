using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental
{
    /// <summary>
    /// Output data returned after returning a rental.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnRentalOutput"/> class.
    /// </remarks>
    /// <param name="rentalId">The rental identifier.</param>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="returnedAt">The return timestamp.</param>
    public sealed class ReturnRentalOutput(Guid rentalId, Guid vehicleId, DateTime returnedAt) : IUseCaseOutput
    {
        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        public Guid RentalId { get; } = rentalId;

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the return timestamp.
        /// </summary>
        public DateTime ReturnedAt { get; } = returnedAt;
    }
}
