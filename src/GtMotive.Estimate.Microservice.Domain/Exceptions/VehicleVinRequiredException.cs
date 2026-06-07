namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a vehicle VIN is required but not provided.
    /// </summary>
    public sealed class VehicleVinRequiredException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleVinRequiredException"/> class.
        /// </summary>
        public VehicleVinRequiredException()
            : base("Vehicle VIN is required.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleVinRequiredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public VehicleVinRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleVinRequiredException"/> class with a specified error message and a inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public VehicleVinRequiredException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
