using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Microsoft.ApplicationInsights;

namespace GtMotive.Estimate.Microservice.Infrastructure.Telemetry
{
    [ExcludeFromCodeCoverage]
    public class AppTelemetry(TelemetryClient telemetry) : ITelemetry
    {
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            telemetry.TrackEvent(eventName, properties, metrics);
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            telemetry.TrackMetric(name, value, properties);
        }
    }
}
