using System;
using GtMotive.Estimate.Microservice.Domain.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Rentals.Enums;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots
{
    /// <summary>
    /// Represents a vehicle rental.
    /// </summary>
    public sealed class Rental
    {
        private Rental(
            Guid id,
            Guid vehicleId,
            string personDocumentId,
            string personName,
            DateTime rentedAt)
        {
            Id = id;
            VehicleId = vehicleId;
            PersonDocumentId = personDocumentId;
            PersonName = personName;
            RentedAt = rentedAt;
            Status = RentalStatus.Active;
        }

        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the rented vehicle identifier.
        /// </summary>
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the person document identifier.
        /// </summary>
        public string PersonDocumentId { get; }

        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string PersonName { get; }

        /// <summary>
        /// Gets the rent timestamp.
        /// </summary>
        public DateTime RentedAt { get; }

        /// <summary>
        /// Gets the rental status.
        /// </summary>
        public RentalStatus Status { get; private set; }

        /// <summary>
        /// Gets the return timestamp.
        /// </summary>
        public DateTime? ReturnedAt { get; private set; }

        /// <summary>
        /// Creates a vehicle rental.
        /// </summary>
        /// <param name="id">The rental identifier.</param>
        /// <param name="vehicleId">The vehicle identifier.</param>
        /// <param name="personDocumentId">The person document identifier.</param>
        /// <param name="personName">The person name.</param>
        /// <param name="rentedAt">The rent timestamp.</param>
        /// <returns>The rental.</returns>
        public static Rental Create(
            Guid id,
            Guid vehicleId,
            string personDocumentId,
            string personName,
            DateTime rentedAt)
        {
            if (id == Guid.Empty)
            {
                throw new RentalIdRequiredException();
            }

            if (vehicleId == Guid.Empty)
            {
                throw new VehicleIdRequiredException();
            }

            if (string.IsNullOrWhiteSpace(personDocumentId))
            {
                throw new PersonDocumentIdRequiredException();
            }

            if (string.IsNullOrWhiteSpace(personName))
            {
                throw new PersonNameRequiredException();
            }

            return new Rental(id, vehicleId, personDocumentId, personName, rentedAt);
        }

        /// <summary>
        /// Rehydrates a persisted rental.
        /// </summary>
        /// <param name="id">The rental identifier.</param>
        /// <param name="vehicleId">The vehicle identifier.</param>
        /// <param name="personDocumentId">The person document identifier.</param>
        /// <param name="personName">The person name.</param>
        /// <param name="rentedAt">The rent timestamp.</param>
        /// <param name="status">The rental status.</param>
        /// <param name="returnedAt">The return timestamp.</param>
        /// <returns>The rehydrated rental.</returns>
        public static Rental Rehydrate(
            Guid id,
            Guid vehicleId,
            string personDocumentId,
            string personName,
            DateTime rentedAt,
            RentalStatus status,
            DateTime? returnedAt)
        {
            var rental = new Rental(id, vehicleId, personDocumentId, personName, rentedAt)
            {
                Status = status,
                ReturnedAt = returnedAt,
            };

            return rental;
        }

        /// <summary>
        /// Returns the rental.
        /// </summary>
        /// <param name="personDocumentId">The person document identifier.</param>
        /// <param name="returnedAt">The return timestamp.</param>
        public void Return(string personDocumentId, DateTime returnedAt)
        {
            if (Status == RentalStatus.Returned)
            {
                throw new RentalAlreadyReturnedException();
            }

            if (PersonDocumentId != personDocumentId)
            {
                throw new RentalDoesNotBelongToPersonException();
            }

            Status = RentalStatus.Returned;
            ReturnedAt = returnedAt;
        }
    }
}
