namespace GtMotive.Estimate.Microservice.Infrastructure
{
    /// <summary>
    /// Defines the backing infrastructure provider used by the application.
    /// </summary>
    public enum InfrastructureProvider
    {
        /// <summary>
        /// Uses in-memory repositories and telemetry.
        /// </summary>
        InMemory = 0,

        /// <summary>
        /// Uses MongoDB repositories and production telemetry.
        /// </summary>
        Mongo = 1,
    }
}
