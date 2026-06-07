namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a rental does not belong to the specified person.
    /// </summary>
    public sealed class RentalDoesNotBelongToPersonException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentalDoesNotBelongToPersonException"/> class.
        /// </summary>
        public RentalDoesNotBelongToPersonException()
            : base("Rental does not belong to the specified person.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalDoesNotBelongToPersonException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RentalDoesNotBelongToPersonException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalDoesNotBelongToPersonException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RentalDoesNotBelongToPersonException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
