using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle
{
    /// <summary>
    /// Input data for creating a fleet vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleInput"/> class.
    /// </remarks>
    /// <param name="vin">The vehicle VIN.</param>
    /// <param name="brand">The vehicle brand.</param>
    /// <param name="model">The vehicle model.</param>
    /// <param name="manufacturingDate">The manufacturing date.</param>
    public sealed class CreateVehicleInput(string vin, string brand, string model, DateOnly manufacturingDate) : IUseCaseInput
    {
        /// <summary>
        /// Gets the vehicle VIN.
        /// </summary>
        public string Vin { get; } = vin;

        /// <summary>
        /// Gets the vehicle brand.
        /// </summary>
        public string Brand { get; } = brand;

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        public string Model { get; } = model;

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly ManufacturingDate { get; } = manufacturingDate;
    }
}
