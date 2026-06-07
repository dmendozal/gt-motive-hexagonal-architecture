using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public class MongoService(IOptions<MongoDbSettings> options)
    {
        private readonly MongoDbSettings settings = options.Value;

        public MongoClient MongoClient { get; } = new(options.Value.ConnectionString);

        public IMongoDatabase Database => MongoClient.GetDatabase(settings.MongoDbDatabaseName);
    }
}
