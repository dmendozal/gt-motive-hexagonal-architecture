using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Rentals.RentVehicle
{
    /// <summary>
    /// Handles rent vehicle HTTP requests.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleRequestHandler"/> class.
    /// </remarks>
    /// <param name="useCase">The rent vehicle use case.</param>
    /// <param name="presenter">The rent vehicle presenter.</param>
    public sealed class RentVehicleRequestHandler(
        RentVehicleUseCase useCase,
        RentVehiclePresenter presenter) : IRequestHandler<RentVehicleRequest, IWebApiPresenter>
    {
        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(RentVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await useCase.Execute(new RentVehicleInput(
                request.VehicleId,
                request.PersonDocumentId,
                request.PersonName));

            return presenter;
        }
    }
}
