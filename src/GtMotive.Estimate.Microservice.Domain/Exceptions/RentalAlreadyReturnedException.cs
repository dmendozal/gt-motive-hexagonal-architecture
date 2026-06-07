namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when attempting to return a rental that has already been returned.
    /// </summary>
    public sealed class RentalAlreadyReturnedException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAlreadyReturnedException"/> class.
        /// </summary>
        public RentalAlreadyReturnedException()
            : base("Rental has already been returned.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAlreadyReturnedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RentalAlreadyReturnedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAlreadyReturnedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RentalAlreadyReturnedException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
