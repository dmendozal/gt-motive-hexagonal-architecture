using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Gets available fleet vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="outputPort">The output port.</param>
    public sealed class GetAvailableVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IGetAvailableVehiclesOutputPort outputPort) : IUseCase<GetAvailableVehiclesInput>
    {
        /// <summary>
        /// Executes the use case.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(GetAvailableVehiclesInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicles = await vehicleRepository.GetAvailable();
            var availableVehicles = new List<AvailableVehicleOutput>();

            foreach (var vehicle in vehicles)
            {
                availableVehicles.Add(new AvailableVehicleOutput(
                    vehicle.Id.Value,
                    vehicle.Vin.Value,
                    vehicle.Brand,
                    vehicle.Model,
                    vehicle.ManufacturingDate));
            }

            outputPort.StandardHandle(new GetAvailableVehiclesOutput(availableVehicles));
        }
    }
}
