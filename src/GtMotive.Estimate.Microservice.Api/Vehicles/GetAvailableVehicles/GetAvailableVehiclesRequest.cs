using GtMotive.Estimate.Microservice.Api.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Request model for getting available fleet vehicles.
    /// </summary>
    public sealed class GetAvailableVehiclesRequest : IRequest<IWebApiPresenter>
    {
    }
}
