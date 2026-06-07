namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a person document id is required but not provided.
    /// </summary>
    public sealed class PersonDocumentIdRequiredException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDocumentIdRequiredException"/> class.
        /// </summary>
        public PersonDocumentIdRequiredException()
            : base("Person document id is required.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDocumentIdRequiredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PersonDocumentIdRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDocumentIdRequiredException"/> class with a specified error message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PersonDocumentIdRequiredException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
