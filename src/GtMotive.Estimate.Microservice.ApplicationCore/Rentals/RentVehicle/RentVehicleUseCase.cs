using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle
{
    /// <summary>
    /// Rents a fleet vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="rentalRepository">The rental repository.</param>
    /// <param name="clock">The clock.</param>
    /// <param name="outputPort">The output port.</param>
    public sealed class RentVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IClock clock,
        IRentVehicleOutputPort outputPort) : IUseCase<RentVehicleInput>
    {
        /// <summary>
        /// Executes the use case.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicle = await vehicleRepository.GetById(input.VehicleId);
            if (vehicle is null)
            {
                outputPort.NotFoundHandle("Vehicle was not found.");
                return;
            }

            if (await rentalRepository.HasActiveRentalForPerson(input.PersonDocumentId))
            {
                throw new PersonAlreadyHasActiveRentalException();
            }

            vehicle.Rent();

            var rental = Rental.Create(
                Guid.NewGuid(),
                vehicle.Id.Value,
                input.PersonDocumentId,
                input.PersonName,
                clock.GetCurrentUtcDateTime());
            await vehicleRepository.Update(vehicle);
            await rentalRepository.Add(rental);

            outputPort.StandardHandle(new RentVehicleOutput(
                rental.Id,
                rental.VehicleId,
                rental.PersonDocumentId,
                rental.RentedAt));
        }
    }
}
