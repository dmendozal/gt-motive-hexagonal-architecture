using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental
{
    /// <summary>
    /// Input data for returning a rental.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnRentalInput"/> class.
    /// </remarks>
    /// <param name="rentalId">The rental identifier.</param>
    /// <param name="personDocumentId">The person document identifier.</param>
    public sealed class ReturnRentalInput(Guid rentalId, string personDocumentId) : IUseCaseInput
    {
        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        public Guid RentalId { get; } = rentalId;

        /// <summary>
        /// Gets the person document identifier.
        /// </summary>
        public string PersonDocumentId { get; } = personDocumentId;
    }
}
