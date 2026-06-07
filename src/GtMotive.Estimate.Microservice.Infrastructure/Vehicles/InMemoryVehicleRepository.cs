using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Infrastructure.Vehicles
{
    /// <summary>
    /// In-memory vehicle repository adapter.
    /// </summary>
    public sealed class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly Lock _syncRoot = new();
        private readonly List<Vehicle> _vehicles = new();

        /// <inheritdoc />
        public Task<bool> ExistsByVin(Vin vin)
        {
            lock (_syncRoot)
            {
                return Task.FromResult(_vehicles.Exists(vehicle => vehicle.Vin.Value == vin.Value));
            }
        }

        /// <inheritdoc />
        public Task Add(Vehicle vehicle)
        {
            lock (_syncRoot)
            {
                _vehicles.Add(vehicle);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<Vehicle> GetById(Guid vehicleId)
        {
            lock (_syncRoot)
            {
                return Task.FromResult(_vehicles.Find(vehicle => vehicle.Id.Value == vehicleId));
            }
        }

        /// <inheritdoc />
        public Task Update(Vehicle vehicle)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<Vehicle>> GetAvailable()
        {
            lock (_syncRoot)
            {
                IReadOnlyCollection<Vehicle> availableVehicles =
                    _vehicles.FindAll(static vehicle => vehicle.Status == VehicleStatus.Available);

                return Task.FromResult(availableVehicles);
            }
        }
    }
}
