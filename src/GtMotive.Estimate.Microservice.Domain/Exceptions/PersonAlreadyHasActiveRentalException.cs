namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a person already has an active rental and cannot rent another vehicle.
    /// </summary>
    public sealed class PersonAlreadyHasActiveRentalException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonAlreadyHasActiveRentalException"/> class.
        /// </summary>
        public PersonAlreadyHasActiveRentalException()
            : base("Person already has an active rental.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonAlreadyHasActiveRentalException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PersonAlreadyHasActiveRentalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonAlreadyHasActiveRentalException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PersonAlreadyHasActiveRentalException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
