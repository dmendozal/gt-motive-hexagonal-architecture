using System;
using GtMotive.Estimate.Microservice.Domain.Common.Errors;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots
{
    /// <summary>
    /// Represents a fleet vehicle.
    /// </summary>
    public sealed class Vehicle
    {
        private Vehicle(VehicleId id, Vin vin, string brand, string model, DateOnly manufacturingDate)
        {
            Id = id;
            Vin = vin;
            Brand = brand;
            Model = model;
            ManufacturingDate = manufacturingDate;
            Status = VehicleStatus.Available;
        }

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public VehicleId Id { get; }

        /// <summary>
        /// Gets the vehicle VIN.
        /// </summary>
        public Vin Vin { get; }

        /// <summary>
        /// Gets the vehicle brand.
        /// </summary>
        public string Brand { get; }

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly ManufacturingDate { get; }

        /// <summary>
        /// Gets the vehicle status.
        /// </summary>
        public VehicleStatus Status { get; private set; }

        /// <summary>
        /// Creates a fleet vehicle.
        /// </summary>
        /// <param name="id">The vehicle identifier.</param>
        /// <param name="vin">The vehicle VIN.</param>
        /// <param name="brand">The vehicle brand.</param>
        /// <param name="model">The vehicle model.</param>
        /// <param name="manufacturingDate">The manufacturing date.</param>
        /// <param name="currentDate">The current UTC date.</param>
        /// <returns>The created vehicle.</returns>
        public static Vehicle Create(
            Guid id,
            string vin,
            string brand,
            string model,
            DateOnly manufacturingDate,
            DateOnly currentDate)
        {
            var vehicleId = VehicleId.Create(id);
            var vehicleVin = Vin.Create(vin);

            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new DomainException(Errors.VehicleBrandRequired);
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new DomainException(Errors.VehicleModelRequired);
            }

            if (manufacturingDate < currentDate.AddYears(-5))
            {
                throw new DomainException(Errors.VehicleTooOld);
            }

            return new Vehicle(vehicleId, vehicleVin, brand, model, manufacturingDate);
        }

        /// <summary>
        /// Rents the vehicle.
        /// </summary>
        public void Rent()
        {
            if (Status != VehicleStatus.Available)
            {
                throw new DomainException(Errors.VehicleAlreadyRented);
            }

            Status = VehicleStatus.Rented;
        }

        /// <summary>
        /// Makes the vehicle available.
        /// </summary>
        public void MakeAvailable()
        {
            Status = VehicleStatus.Available;
        }
    }
}
