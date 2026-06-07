using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental
{
    /// <summary>
    /// Output port for the return rental use case.
    /// </summary>
    public interface IReturnRentalOutputPort : IOutputPortStandard<ReturnRentalOutput>, IOutputPortNotFound
    {
    }
}
