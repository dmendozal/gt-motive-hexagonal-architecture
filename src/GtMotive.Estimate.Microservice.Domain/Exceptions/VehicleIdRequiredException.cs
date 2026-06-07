namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a vehicle id is required but not provided.
    /// </summary>
    public sealed class VehicleIdRequiredException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleIdRequiredException"/> class.
        /// </summary>
        public VehicleIdRequiredException()
            : base("Vehicle id is required.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleIdRequiredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public VehicleIdRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleIdRequiredException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public VehicleIdRequiredException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
