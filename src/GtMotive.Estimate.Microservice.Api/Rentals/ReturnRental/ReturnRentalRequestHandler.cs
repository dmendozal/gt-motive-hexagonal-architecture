using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental
{
    /// <summary>
    /// Handles return rental HTTP requests.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnRentalRequestHandler"/> class.
    /// </remarks>
    /// <param name="useCase">The return rental use case.</param>
    /// <param name="presenter">The return rental presenter.</param>
    public sealed class ReturnRentalRequestHandler(
        ReturnRentalUseCase useCase,
        ReturnRentalPresenter presenter) : IRequestHandler<ReturnRentalRequest, IWebApiPresenter>
    {
        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(ReturnRentalRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await useCase.Execute(new ReturnRentalInput(request.RentalId, request.PersonDocumentId));
            return presenter;
        }
    }
}
