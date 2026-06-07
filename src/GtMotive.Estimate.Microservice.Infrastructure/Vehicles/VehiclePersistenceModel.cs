using System;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.Vehicles
{
    [BsonIgnoreExtraElements]
    public sealed class VehiclePersistenceModel
    {
        public Guid Id { get; set; }

        public string Vin { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public DateTime ManufacturingDate { get; set; }

        public VehicleStatus Status { get; set; }

        public static VehiclePersistenceModel FromDomain(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            return new VehiclePersistenceModel
            {
                Id = vehicle.Id.Value,
                Vin = vehicle.Vin.Value,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ManufacturingDate = vehicle.ManufacturingDate.ToDateTime(TimeOnly.MinValue),
                Status = vehicle.Status,
            };
        }

        public Vehicle ToDomain()
        {
            var vin = GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects.Vin.Create(Vin);

            return Vehicle.Rehydrate(
                VehicleId.Create(Id),
                vin,
                Brand,
                Model,
                DateOnly.FromDateTime(ManufacturingDate),
                Status);
        }
    }
}
