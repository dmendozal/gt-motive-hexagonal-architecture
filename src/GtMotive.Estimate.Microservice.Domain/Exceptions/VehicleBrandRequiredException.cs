namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a vehicle brand is required but not provided.
    /// </summary>
    public sealed class VehicleBrandRequiredException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleBrandRequiredException"/> class.
        /// </summary>
        public VehicleBrandRequiredException()
            : base("Vehicle brand is required.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleBrandRequiredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public VehicleBrandRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleBrandRequiredException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public VehicleBrandRequiredException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
