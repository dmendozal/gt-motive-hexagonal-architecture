using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Domain.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public sealed class CreateVehicleUseCaseTests
    {
        [Fact]
        public async Task ExecuteWhenVehicleIsValidAddsVehicleAndWritesStandardOutput()
        {
            var repository = new VehicleRepositoryStub();
            var clock = new ClockStub(new DateOnly(2026, 6, 7));
            var outputPort = new CreateVehicleOutputPortSpy();
            var useCase = new CreateVehicleUseCase(repository, clock, outputPort);
            var input = new CreateVehicleInput("VIN-001", "Toyota", "Corolla", new DateOnly(2023, 1, 10));

            await useCase.Execute(input);

            outputPort.StandardOutput.Should().NotBeNull();
            outputPort.StandardOutput.Vin.Should().Be("VIN-001");
            repository.Vehicles.Should().ContainSingle();
            repository.Vehicles[0].Status.Should().Be(VehicleStatus.Available);
        }

        [Fact]
        public async Task ExecuteWhenVinAlreadyExistsReturnsConflict()
        {
            var repository = new VehicleRepositoryStub();
            var clock = new ClockStub(new DateOnly(2026, 6, 7));
            var outputPort = new CreateVehicleOutputPortSpy();
            var useCase = new CreateVehicleUseCase(repository, clock, outputPort);
            var existingVehicle = Vehicle.Create(
                Guid.NewGuid(),
                "VIN-001",
                "Toyota",
                "Corolla",
                new DateOnly(2023, 1, 10),
                clock.GetCurrentDate());
            await repository.Add(existingVehicle);
            var input = new CreateVehicleInput("VIN-001", "Ford", "Focus", new DateOnly(2024, 2, 20));

            await useCase.Execute(input);

            outputPort.ConflictMessage.Should().Be("Vehicle already exists.");
            outputPort.StandardOutput.Should().BeNull();
            repository.Vehicles.Should().ContainSingle();
        }

        [Fact]
        public async Task ExecuteWhenVehicleManufacturingDateExceedsFiveYearsThrowsDomainException()
        {
            var repository = new VehicleRepositoryStub();
            var clock = new ClockStub(new DateOnly(2026, 6, 7));
            var outputPort = new CreateVehicleOutputPortSpy();
            var useCase = new CreateVehicleUseCase(repository, clock, outputPort);
            var input = new CreateVehicleInput("VIN-001", "Toyota", "Corolla", new DateOnly(2021, 6, 6));

            Func<Task> act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<VehicleManufacturingDateExceededException>();
            outputPort.StandardOutput.Should().BeNull();
            repository.Vehicles.Should().BeEmpty();
        }

        private sealed class ClockStub(DateOnly currentDate) : IClock
        {
            public DateOnly GetCurrentDate()
            {
                return currentDate;
            }

            public DateTime GetCurrentUtcDateTime()
            {
                return currentDate.ToDateTime(TimeOnly.MinValue);
            }
        }

        private sealed class CreateVehicleOutputPortSpy : ICreateVehicleOutputPort
        {
            public CreateVehicleOutput StandardOutput { get; private set; }

            public string ConflictMessage { get; private set; }

            public void StandardHandle(CreateVehicleOutput response)
            {
                StandardOutput = response;
            }

            public void ConflictHandle(string message)
            {
                ConflictMessage = message;
            }
        }

        private sealed class VehicleRepositoryStub : IVehicleRepository
        {
            public List<Vehicle> Vehicles { get; } = new List<Vehicle>();

            public Task<bool> ExistsByVin(Vin vin)
            {
                var exists = Vehicles.Exists(vehicle => vehicle.Vin.Value == vin.Value);
                return Task.FromResult(exists);
            }

            public Task Add(Vehicle vehicle)
            {
                Vehicles.Add(vehicle);
                return Task.CompletedTask;
            }

            public Task<Vehicle> GetById(Guid vehicleId)
            {
                return Task.FromResult(Vehicles.Find(vehicle => vehicle.Id.Value == vehicleId));
            }

            public Task Update(Vehicle vehicle)
            {
                return Task.CompletedTask;
            }

            public Task<IReadOnlyCollection<Vehicle>> GetAvailable()
            {
                IReadOnlyCollection<Vehicle> availableVehicles =
                    Vehicles.FindAll(static vehicle => vehicle.Status == VehicleStatus.Available);

                return Task.FromResult(availableVehicles);
            }
        }
    }
}
