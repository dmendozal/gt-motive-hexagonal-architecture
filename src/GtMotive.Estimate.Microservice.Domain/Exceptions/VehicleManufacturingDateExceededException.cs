namespace GtMotive.Estimate.Microservice.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a vehicle's manufacturing date exceeds 5 years.
    /// </summary>
    public sealed class VehicleManufacturingDateExceededException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturingDateExceededException"/> class.
        /// </summary>
        public VehicleManufacturingDateExceededException()
            : base("The vehicle manufacturing date cannot be older than 5 years.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturingDateExceededException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public VehicleManufacturingDateExceededException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturingDateExceededException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public VehicleManufacturingDateExceededException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
