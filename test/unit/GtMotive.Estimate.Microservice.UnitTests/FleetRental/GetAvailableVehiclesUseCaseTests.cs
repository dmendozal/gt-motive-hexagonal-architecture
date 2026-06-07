using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.FleetRental
{
    public sealed class GetAvailableVehiclesUseCaseTests
    {
        [Fact]
        public async Task ExecuteWhenAvailableVehiclesExistWritesAvailableVehiclesOutput()
        {
            var repository = new VehicleRepositoryStub();
            var outputPort = new GetAvailableVehiclesOutputPortSpy();
            var useCase = new GetAvailableVehiclesUseCase(repository, outputPort);
            var vehicle = Vehicle.Create(
                Guid.NewGuid(),
                "VIN-001",
                "Toyota",
                "Corolla",
                new DateOnly(2023, 1, 10),
                new DateOnly(2026, 6, 7));
            await repository.Add(vehicle);

            await useCase.Execute(new GetAvailableVehiclesInput());

            outputPort.StandardOutput.Should().NotBeNull();
            outputPort.StandardOutput.Vehicles.Should().ContainSingle();
            outputPort.StandardOutput.Vehicles.Should().ContainSingle(static availableVehicle =>
                availableVehicle.Vin == "VIN-001"
                && availableVehicle.Brand == "Toyota"
                && availableVehicle.Model == "Corolla");
        }

        [Fact]
        public async Task ExecuteWhenNoAvailableVehiclesExistWritesEmptyOutput()
        {
            var repository = new VehicleRepositoryStub();
            var outputPort = new GetAvailableVehiclesOutputPortSpy();
            var useCase = new GetAvailableVehiclesUseCase(repository, outputPort);

            await useCase.Execute(new GetAvailableVehiclesInput());

            outputPort.StandardOutput.Should().NotBeNull();
            outputPort.StandardOutput.Vehicles.Should().BeEmpty();
        }

        private sealed class GetAvailableVehiclesOutputPortSpy : IGetAvailableVehiclesOutputPort
        {
            public GetAvailableVehiclesOutput StandardOutput { get; private set; }

            public void StandardHandle(GetAvailableVehiclesOutput response)
            {
                StandardOutput = response;
            }
        }

        private sealed class VehicleRepositoryStub : IVehicleRepository
        {
            private readonly List<Vehicle> vehicles = new();

            public Task<bool> ExistsByVin(Vin vin)
            {
                var exists = vehicles.Exists(vehicle => vehicle.Vin.Value == vin.Value);
                return Task.FromResult(exists);
            }

            public Task Add(Vehicle vehicle)
            {
                vehicles.Add(vehicle);
                return Task.CompletedTask;
            }

            public Task<Vehicle> GetById(Guid vehicleId)
            {
                return Task.FromResult(vehicles.Find(vehicle => vehicle.Id.Value == vehicleId));
            }

            public Task Update(Vehicle vehicle)
            {
                return Task.CompletedTask;
            }

            public Task<IReadOnlyCollection<Vehicle>> GetAvailable()
            {
                IReadOnlyCollection<Vehicle> availableVehicles =
                    vehicles.FindAll(static vehicle => vehicle.Status == VehicleStatus.Available);

                return Task.FromResult(availableVehicles);
            }
        }
    }
}
