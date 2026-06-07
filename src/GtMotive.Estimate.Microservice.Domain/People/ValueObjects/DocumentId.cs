using GtMotive.Estimate.Microservice.Domain.Common.Errors;

namespace GtMotive.Estimate.Microservice.Domain.People.ValueObjects
{
    /// <summary>
    /// Person document identifier value object.
    /// </summary>
    public sealed class DocumentId
    {
        private DocumentId(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the document identifier value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a document identifier.
        /// </summary>
        /// <param name="value">The document identifier value.</param>
        /// <returns>The document identifier.</returns>
        public static DocumentId Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(Errors.PersonDocumentIdRequired);
            }

            return new DocumentId(value);
        }
    }
}
