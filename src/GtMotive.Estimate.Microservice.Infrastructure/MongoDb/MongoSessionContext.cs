using System.Threading;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public sealed class MongoSessionContext
    {
        private readonly AsyncLocal<IClientSessionHandle> currentSession = new();

        public IClientSessionHandle CurrentSession
        {
            get => currentSession.Value;
            set => currentSession.Value = value;
        }
    }
}
