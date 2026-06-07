using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Rentals.Enums;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Rentals
{
    public sealed class MongoRentalRepository : IRentalRepository
    {
        private const string CollectionName = "rentals";
        private readonly IMongoCollection<RentalPersistenceModel> _collection;

        public MongoRentalRepository(MongoService mongoService)
        {
            ArgumentNullException.ThrowIfNull(mongoService);

            _collection = mongoService.Database.GetCollection<RentalPersistenceModel>(CollectionName);

            var activeRentalIndex = new CreateIndexModel<RentalPersistenceModel>(
                Builders<RentalPersistenceModel>.IndexKeys.Ascending(static document => document.PersonDocumentId),
                new CreateIndexOptions<RentalPersistenceModel>
                {
                    Unique = true,
                    Name = "ux_rentals_active_person_document_id",
                    PartialFilterExpression = Builders<RentalPersistenceModel>.Filter.Eq(static document => document.Status, RentalStatus.Active),
                });

            _ = _collection.Indexes.CreateOne(activeRentalIndex);
        }

        public async Task<bool> HasActiveRentalForPerson(string personDocumentId)
        {
            var exists = await _collection.Find(document =>
                document.PersonDocumentId == personDocumentId && document.Status == RentalStatus.Active).AnyAsync();

            return exists;
        }

        public Task Add(Rental rental)
        {
            return _collection.InsertOneAsync(RentalPersistenceModel.FromDomain(rental));
        }

        public async Task<Rental> GetById(Guid rentalId)
        {
            var document = await _collection.Find(document => document.Id == rentalId).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public Task Update(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            var filter = Builders<RentalPersistenceModel>.Filter.Eq(static document => document.Id, rental.Id);
            return _collection.ReplaceOneAsync(filter, RentalPersistenceModel.FromDomain(rental));
        }
    }
}
