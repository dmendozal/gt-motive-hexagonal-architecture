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
        private readonly MongoSessionContext _sessionContext;

        public MongoVehicleRepository(MongoService mongoService, MongoSessionContext sessionContext)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            ArgumentNullException.ThrowIfNull(sessionContext);

            _sessionContext = sessionContext;
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
            ArgumentNullException.ThrowIfNull(vin);

            var session = _sessionContext.CurrentSession;
            var filter = Builders<VehiclePersistenceModel>.Filter.Eq(static document => document.Vin, vin.Value);
            var exists = session is null ?
                await _collection.Find(filter).AnyAsync() :
                await _collection.Find(session, filter).AnyAsync();

            return exists;
        }

        public Task Add(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            var session = _sessionContext.CurrentSession;
            var document = VehiclePersistenceModel.FromDomain(vehicle);

            return session is null ?
                _collection.InsertOneAsync(document) :
                _collection.InsertOneAsync(session, document);
        }

        public async Task<Vehicle> GetById(Guid vehicleId)
        {
            var session = _sessionContext.CurrentSession;
            var filter = Builders<VehiclePersistenceModel>.Filter.Eq(static document => document.Id, vehicleId);
            var document = session is null ?
                await _collection.Find(filter).FirstOrDefaultAsync() :
                await _collection.Find(session, filter).FirstOrDefaultAsync();

            return document?.ToDomain();
        }

        public Task Update(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            var session = _sessionContext.CurrentSession;
            var filter = Builders<VehiclePersistenceModel>.Filter.Eq(static document => document.Id, vehicle.Id.Value);
            var document = VehiclePersistenceModel.FromDomain(vehicle);

            return session is null ?
                _collection.ReplaceOneAsync(filter, document) :
                _collection.ReplaceOneAsync(session, filter, document);
        }

        public async Task<IReadOnlyCollection<Vehicle>> GetAvailable()
        {
            var session = _sessionContext.CurrentSession;
            var filter = Builders<VehiclePersistenceModel>.Filter.Eq(static document => document.Status, VehicleStatus.Available);
            var documents = session is null ?
                await _collection.Find(filter).ToListAsync() :
                await _collection.Find(session, filter).ToListAsync();

            IReadOnlyCollection<Vehicle> availableVehicles = documents.ConvertAll(static document => document.ToDomain());
            return availableVehicles;
        }
    }
}
