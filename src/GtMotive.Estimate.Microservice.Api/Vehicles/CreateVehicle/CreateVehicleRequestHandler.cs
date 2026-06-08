using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Vehicles.CreateVehicle
{
    /// <summary>
    /// Handles create vehicle HTTP requests.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleRequestHandler"/> class.
    /// </remarks>
    /// <param name="useCase">The create vehicle use case.</param>
    /// <param name="presenter">The create vehicle presenter.</param>
    public sealed class CreateVehicleRequestHandler(
        CreateVehicleUseCase useCase,
        CreateVehiclePresenter presenter) : IRequestHandler<CreateVehicleRequest, IWebApiPresenter>
    {
        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(CreateVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new CreateVehicleInput(
                request.Vin,
                request.Brand,
                request.Model,
                request.ManufacturingDate);

            await useCase.Execute(input);
            return presenter;
        }
    }
}
