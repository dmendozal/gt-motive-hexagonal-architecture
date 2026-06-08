using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.InMemory
{
    public sealed class InMemoryUnitOfWork : IUnitOfWork
    {
        public async Task Execute(Func<Task> operation)
        {
            ArgumentNullException.ThrowIfNull(operation);

            await operation();
        }

        public Task<int> Save()
        {
            return Task.FromResult(0);
        }
    }
}
