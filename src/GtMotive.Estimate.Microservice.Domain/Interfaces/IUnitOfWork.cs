using System;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Unit Of Work. Should only be used by Use Cases.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Executes an operation inside a unit of work.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Execute(Func<Task> operation);

        /// <summary>
        /// Applies all database changes.
        /// </summary>
        /// <returns>Number of affected rows.</returns>
        Task<int> Save();
    }
}
