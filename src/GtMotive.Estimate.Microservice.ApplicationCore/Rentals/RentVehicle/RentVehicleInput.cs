using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle
{
    /// <summary>
    /// Input data for renting a vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleInput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The vehicle identifier.</param>
    /// <param name="personDocumentId">The person document identifier.</param>
    /// <param name="personName">The person name.</param>
    public sealed class RentVehicleInput(Guid vehicleId, string personDocumentId, string personName) : IUseCaseInput
    {
        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the person document identifier.
        /// </summary>
        public string PersonDocumentId { get; } = personDocumentId;

        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string PersonName { get; } = personName;
    }
}
