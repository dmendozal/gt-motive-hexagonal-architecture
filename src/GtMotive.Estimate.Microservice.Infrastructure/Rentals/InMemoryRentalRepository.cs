using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Rentals.Enums;

namespace GtMotive.Estimate.Microservice.Infrastructure.Rentals
{
    /// <summary>
    /// In-memory rental repository adapter.
    /// </summary>
    public sealed class InMemoryRentalRepository : IRentalRepository
    {
        private readonly Lock _syncRoot = new();
        private readonly List<Rental> _rentals = new();

        /// <inheritdoc />
        public Task<bool> HasActiveRentalForPerson(string personDocumentId)
        {
            lock (_syncRoot)
            {
                var hasActiveRental = _rentals.Exists(rental =>
                    rental.PersonDocumentId == personDocumentId && rental.Status == RentalStatus.Active);

                return Task.FromResult(hasActiveRental);
            }
        }

        /// <inheritdoc />
        public Task Add(Rental rental)
        {
            lock (_syncRoot)
            {
                _rentals.Add(rental);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<Rental> GetById(Guid rentalId)
        {
            lock (_syncRoot)
            {
                return Task.FromResult(_rentals.Find(rental => rental.Id == rentalId));
            }
        }

        /// <inheritdoc />
        public Task Update(Rental rental)
        {
            return Task.CompletedTask;
        }
    }
}
