using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GtMotive.Estimate.Microservice.Api.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental
{
    /// <summary>
    /// Request model for returning a rental.
    /// </summary>
    public sealed class ReturnRentalRequest : IRequest<IWebApiPresenter>
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        [JsonIgnore]
        public Guid RentalId { get; set; }

        /// <summary>
        /// Gets or sets the person document identifier.
        /// </summary>
        [Required]
        public string PersonDocumentId { get; set; }
    }
}
