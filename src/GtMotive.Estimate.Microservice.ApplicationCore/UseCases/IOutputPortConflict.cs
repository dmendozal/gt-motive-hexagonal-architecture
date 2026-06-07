namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Interface to define the Conflict Output Port.
    /// </summary>
    public interface IOutputPortConflict
    {
        /// <summary>
        /// Informs there is a conflict in the resource.
        /// </summary>
        /// <param name="message">Text description.</param>
        void ConflictHandle(string message);
    }
}
