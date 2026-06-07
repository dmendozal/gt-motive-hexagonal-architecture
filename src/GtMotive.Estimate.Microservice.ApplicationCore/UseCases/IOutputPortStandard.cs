namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Interface to define the Standard Output Port.
    /// </summary>
    /// <typeparam name="TUseCaseOutput">Type of the use case response dto.</typeparam>
    public interface IOutputPortStandard<in TUseCaseOutput>
        where TUseCaseOutput : IUseCaseOutput
    {
        /// <summary>
        /// Writes to the Standard Output.
        /// </summary>
        /// <param name="response">The Output Port Message.</param>
        void StandardHandle(TUseCaseOutput response);
    }
}
