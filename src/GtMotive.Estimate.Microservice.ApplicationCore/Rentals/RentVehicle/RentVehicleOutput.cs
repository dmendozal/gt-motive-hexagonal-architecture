using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle
{
    /// <summary>
    /// Output data returned after renting a vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleOutput"/> class.
    /// </remarks>
    /// <param name="rentalId">The rental identifier.</param>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="personDocumentId">The person document identifier.</param>
    /// <param name="rentedAt">The rent timestamp.</param>
    public sealed class RentVehicleOutput(Guid rentalId, Guid vehicleId, string personDocumentId, DateTime rentedAt) : IUseCaseOutput
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
        /// Gets the person document identifier.
        /// </summary>
        public string PersonDocumentId { get; } = personDocumentId;

        /// <summary>
        /// Gets the rent timestamp.
        /// </summary>
        public DateTime RentedAt { get; } = rentedAt;
    }
}
