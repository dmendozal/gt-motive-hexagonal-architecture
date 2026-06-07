using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental
{
    /// <summary>
    /// Request model for returning a rental.
    /// </summary>
    public sealed class ReturnRentalRequest
    {
        /// <summary>
        /// Gets or sets the person document identifier.
        /// </summary>
        [Required]
        public string PersonDocumentId { get; set; }
    }
}
