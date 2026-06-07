using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle
{
    /// <summary>
    /// Creates fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="clock">The clock.</param>
    /// <param name="outputPort">The output port.</param>
    public sealed class CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IClock clock,
        ICreateVehicleOutputPort outputPort) : IUseCase<CreateVehicleInput>
    {
        /// <summary>
        /// Executes the use case.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicleId = Guid.NewGuid();
            var vehicle = Vehicle.Create(
                vehicleId,
                input.Vin,
                input.Brand,
                input.Model,
                input.ManufacturingDate,
                clock.GetCurrentDate());

            if (await vehicleRepository.ExistsByVin(vehicle.Vin))
            {
                outputPort.ConflictHandle("Vehicle already exists.");
                return;
            }

            await vehicleRepository.Add(vehicle);

            outputPort.StandardHandle(new CreateVehicleOutput(vehicle.Id.Value, vehicle.Vin.Value));
        }
    }
}
