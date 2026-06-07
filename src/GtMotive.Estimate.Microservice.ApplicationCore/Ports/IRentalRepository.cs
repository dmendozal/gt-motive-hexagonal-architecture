using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Ports
{
    /// <summary>
    /// Rental persistence port.
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Checks whether the person has an active rental.
        /// </summary>
        /// <param name="personDocumentId">The person document identifier.</param>
        /// <returns>A value indicating whether the person has an active rental.</returns>
        Task<bool> HasActiveRentalForPerson(string personDocumentId);

        /// <summary>
        /// Adds a rental.
        /// </summary>
        /// <param name="rental">The rental to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Rental rental);

        /// <summary>
        /// Gets a rental by its identifier.
        /// </summary>
        /// <param name="rentalId">The rental identifier.</param>
        /// <returns>The rental when found; otherwise null.</returns>
        Task<Rental> GetById(Guid rentalId);

        /// <summary>
        /// Updates a rental.
        /// </summary>
        /// <param name="rental">The rental to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(Rental rental);
    }
}
