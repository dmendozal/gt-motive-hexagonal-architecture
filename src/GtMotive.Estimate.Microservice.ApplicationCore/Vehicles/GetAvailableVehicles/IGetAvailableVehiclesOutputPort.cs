using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Output port for the get available vehicles use case.
    /// </summary>
    public interface IGetAvailableVehiclesOutputPort : IOutputPortStandard<GetAvailableVehiclesOutput>
    {
    }
}
