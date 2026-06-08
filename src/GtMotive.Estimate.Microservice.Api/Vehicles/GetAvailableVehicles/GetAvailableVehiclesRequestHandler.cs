using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Handles get available vehicles HTTP requests.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesRequestHandler"/> class.
    /// </remarks>
    /// <param name="useCase">The get available vehicles use case.</param>
    /// <param name="presenter">The get available vehicles presenter.</param>
    public sealed class GetAvailableVehiclesRequestHandler(
        GetAvailableVehiclesUseCase useCase,
        GetAvailableVehiclesPresenter presenter) : IRequestHandler<GetAvailableVehiclesRequest, IWebApiPresenter>
    {
        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(GetAvailableVehiclesRequest request, CancellationToken cancellationToken)
        {
            await useCase.Execute(new GetAvailableVehiclesInput());
            return presenter;
        }
    }
}
