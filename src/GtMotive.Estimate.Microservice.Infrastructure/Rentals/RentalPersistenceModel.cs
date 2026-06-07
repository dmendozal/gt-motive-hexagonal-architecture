using System;
using GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Rentals.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.Rentals
{
    [BsonIgnoreExtraElements]
    public sealed class RentalPersistenceModel
    {
        public Guid Id { get; set; }

        public Guid VehicleId { get; set; }

        public string PersonDocumentId { get; set; } = string.Empty;

        public string PersonName { get; set; } = string.Empty;

        public DateTime RentedAt { get; set; }

        public RentalStatus Status { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public static RentalPersistenceModel FromDomain(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            return new RentalPersistenceModel
            {
                Id = rental.Id,
                VehicleId = rental.VehicleId,
                PersonDocumentId = rental.PersonDocumentId,
                PersonName = rental.PersonName,
                RentedAt = rental.RentedAt,
                Status = rental.Status,
                ReturnedAt = rental.ReturnedAt,
            };
        }

        public Rental ToDomain()
        {
            return Rental.Rehydrate(
                Id,
                VehicleId,
                PersonDocumentId,
                PersonName,
                RentedAt,
                Status,
                ReturnedAt);
        }
    }
}
