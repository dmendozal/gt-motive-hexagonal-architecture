namespace GtMotive.Estimate.Microservice.Api.Common
{
    /// <summary>
    /// Error response model.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// </remarks>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public sealed class ErrorResponse(string code, string message)
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; } = code;

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; } = message;
    }
}
