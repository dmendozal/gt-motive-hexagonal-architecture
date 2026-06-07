using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Ports
{
    /// <summary>
    /// Provides the current date and time.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current UTC date.
        /// </summary>
        /// <returns>The current UTC date.</returns>
        DateOnly GetCurrentDate();

        /// <summary>
        /// Gets the current UTC timestamp.
        /// </summary>
        /// <returns>The current UTC timestamp.</returns>
        DateTime GetCurrentUtcDateTime();
    }
}
