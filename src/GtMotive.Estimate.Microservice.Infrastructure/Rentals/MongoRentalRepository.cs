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
        private readonly MongoSessionContext _sessionContext;

        public MongoRentalRepository(MongoService mongoService, MongoSessionContext sessionContext)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            ArgumentNullException.ThrowIfNull(sessionContext);

            _sessionContext = sessionContext;
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
            var session = _sessionContext.CurrentSession;
            var filter = Builders<RentalPersistenceModel>.Filter.And(
                Builders<RentalPersistenceModel>.Filter.Eq(static document => document.PersonDocumentId, personDocumentId),
                Builders<RentalPersistenceModel>.Filter.Eq(static document => document.Status, RentalStatus.Active));

            var exists = session is null ?
                await _collection.Find(filter).AnyAsync() :
                await _collection.Find(session, filter).AnyAsync();

            return exists;
        }

        public Task Add(Rental rental)
        {
            var session = _sessionContext.CurrentSession;
            var document = RentalPersistenceModel.FromDomain(rental);

            return session is null ?
                _collection.InsertOneAsync(document) :
                _collection.InsertOneAsync(session, document);
        }

        public async Task<Rental> GetById(Guid rentalId)
        {
            var session = _sessionContext.CurrentSession;
            var filter = Builders<RentalPersistenceModel>.Filter.Eq(static document => document.Id, rentalId);
            var document = session is null ?
                await _collection.Find(filter).FirstOrDefaultAsync() :
                await _collection.Find(session, filter).FirstOrDefaultAsync();

            return document?.ToDomain();
        }

        public Task Update(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            var session = _sessionContext.CurrentSession;
            var filter = Builders<RentalPersistenceModel>.Filter.Eq(static document => document.Id, rental.Id);
            var document = RentalPersistenceModel.FromDomain(rental);

            return session is null ?
                _collection.ReplaceOneAsync(filter, document) :
                _collection.ReplaceOneAsync(session, filter, document);
        }
    }
}
