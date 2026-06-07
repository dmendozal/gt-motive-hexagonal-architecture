using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Vehicles
{
    public sealed class MongoVehicleRepository : IVehicleRepository
    {
        private const string CollectionName = "vehicles";
        private readonly IMongoCollection<VehiclePersistenceModel> _collection;

        public MongoVehicleRepository(MongoService mongoService)
        {
            ArgumentNullException.ThrowIfNull(mongoService);

            _collection = mongoService.Database.GetCollection<VehiclePersistenceModel>(CollectionName);

            var vinIndex = new CreateIndexModel<VehiclePersistenceModel>(
                Builders<VehiclePersistenceModel>.IndexKeys.Ascending(static document => document.Vin),
                new CreateIndexOptions
                {
                    Unique = true,
                    Name = "ux_vehicles_vin",
                });

            _ = _collection.Indexes.CreateOne(vinIndex);
        }

        public async Task<bool> ExistsByVin(Vin vin)
        {
            var exists = await _collection.Find(document => document.Vin == vin.Value).AnyAsync();
            return exists;
        }

        public Task Add(Vehicle vehicle)
        {
            return _collection.InsertOneAsync(VehiclePersistenceModel.FromDomain(vehicle));
        }

        public async Task<Vehicle> GetById(Guid vehicleId)
        {
            var document = await _collection.Find(document => document.Id == vehicleId).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public Task Update(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            var filter = Builders<VehiclePersistenceModel>.Filter.Eq(static document => document.Id, vehicle.Id.Value);
            return _collection.ReplaceOneAsync(filter, VehiclePersistenceModel.FromDomain(vehicle));
        }

        public async Task<IReadOnlyCollection<Vehicle>> GetAvailable()
        {
            var documents = await _collection.Find(static document => document.Status == VehicleStatus.Available).ToListAsync();
            IReadOnlyCollection<Vehicle> availableVehicles = documents.ConvertAll(static document => document.ToDomain());
            return availableVehicles;
        }
    }
}
