using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public sealed class MongoUnitOfWork(MongoService mongoService, MongoSessionContext sessionContext) : IUnitOfWork
    {
        public async Task Execute(Func<Task> operation)
        {
            ArgumentNullException.ThrowIfNull(operation);

            using var session = await mongoService.MongoClient.StartSessionAsync();
            session.StartTransaction();
            sessionContext.CurrentSession = session;

            try
            {
                await operation();
                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
            finally
            {
                sessionContext.CurrentSession = null;
            }
        }

        public Task<int> Save()
        {
            return Task.FromResult(0);
        }
    }
}
