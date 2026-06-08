using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental
{
    /// <summary>
    /// Returns a rented fleet vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnRentalUseCase"/> class.
    /// </remarks>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="clock">The clock.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    public sealed class ReturnRentalUseCase(
        IRentalRepository rentalRepository,
        IVehicleRepository vehicleRepository,
        IClock clock,
        IUnitOfWork unitOfWork,
        IReturnRentalOutputPort outputPort) : IUseCase<ReturnRentalInput>
    {
        /// <summary>
        /// Executes the use case.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(ReturnRentalInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var rental = await rentalRepository.GetById(input.RentalId);
            if (rental is null)
            {
                outputPort.NotFoundHandle("Rental not found.");
                return;
            }

            ReturnRentalOutput output = null;
            await unitOfWork.Execute(async () =>
            {
                var returnedAt = clock.GetCurrentUtcDateTime();
                rental.Return(input.PersonDocumentId, returnedAt);

                var vehicle = await vehicleRepository.GetById(rental.VehicleId);
                if (vehicle is not null)
                {
                    vehicle.MakeAvailable();
                    await vehicleRepository.Update(vehicle);
                }

                await rentalRepository.Update(rental);
                output = new ReturnRentalOutput(rental.Id, rental.VehicleId, returnedAt);
            });

            outputPort.StandardHandle(output);
        }
    }
}
