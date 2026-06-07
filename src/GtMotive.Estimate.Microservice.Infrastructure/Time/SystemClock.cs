using System;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;

namespace GtMotive.Estimate.Microservice.Infrastructure.Time
{
    /// <summary>
    /// System clock adapter.
    /// </summary>
    public sealed class SystemClock : IClock
    {
        /// <inheritdoc />
        public DateOnly GetCurrentDate()
        {
            return DateOnly.FromDateTime(DateTime.UtcNow);
        }

        /// <inheritdoc />
        public DateTime GetCurrentUtcDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
