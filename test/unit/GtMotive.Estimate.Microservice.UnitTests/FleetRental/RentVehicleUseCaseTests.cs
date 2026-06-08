using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.Domain.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Rentals.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Rentals.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.FleetRental
{
    public sealed class RentVehicleUseCaseTests
    {
        [Fact]
        public async Task ExecuteWhenVehicleIsAvailableAndPersonHasNoActiveRentalCreatesRentalAndRentsVehicle()
        {
            var vehicleRepository = new VehicleRepositoryStub();
            var rentalRepository = new RentalRepositoryStub();
            var outputPort = new RentVehicleOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new RentVehicleUseCase(vehicleRepository, rentalRepository, clock, new UnitOfWorkStub(), outputPort);
            var vehicle = CreateVehicle("VIN-001");
            await vehicleRepository.Add(vehicle);

            await useCase.Execute(new RentVehicleInput(vehicle.Id.Value, "12345678A", "Jane Doe"));

            outputPort.StandardOutput.Should().NotBeNull();
            outputPort.StandardOutput.VehicleId.Should().Be(vehicle.Id.Value);
            outputPort.StandardOutput.PersonDocumentId.Should().Be("12345678A");
            vehicle.Status.Should().Be(VehicleStatus.Rented);
            rentalRepository.Rentals.Should().ContainSingle();
        }

        [Fact]
        public async Task ExecuteWhenVehicleDoesNotExistReturnsNotFound()
        {
            var vehicleRepository = new VehicleRepositoryStub();
            var rentalRepository = new RentalRepositoryStub();
            var outputPort = new RentVehicleOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new RentVehicleUseCase(vehicleRepository, rentalRepository, clock, new UnitOfWorkStub(), outputPort);

            await useCase.Execute(new RentVehicleInput(Guid.NewGuid(), "12345678A", "Jane Doe"));

            outputPort.NotFoundMessage.Should().Be("Vehicle was not found.");
            outputPort.StandardOutput.Should().BeNull();
            rentalRepository.Rentals.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteWhenPersonAlreadyHasActiveRentalThrowsDomainException()
        {
            var vehicleRepository = new VehicleRepositoryStub();
            var rentalRepository = new RentalRepositoryStub();
            var outputPort = new RentVehicleOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new RentVehicleUseCase(vehicleRepository, rentalRepository, clock, new UnitOfWorkStub(), outputPort);
            var vehicle = CreateVehicle("VIN-001");
            await vehicleRepository.Add(vehicle);
            await rentalRepository.Add(Rental.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "12345678A",
                "Jane Doe",
                clock.GetCurrentUtcDateTime()));

            Func<Task> act = () => useCase.Execute(new RentVehicleInput(vehicle.Id.Value, "12345678A", "Jane Doe"));

            await act.Should().ThrowAsync<PersonAlreadyHasActiveRentalException>();
            outputPort.StandardOutput.Should().BeNull();
            vehicle.Status.Should().Be(VehicleStatus.Available);
        }

        [Fact]
        public async Task ExecuteWhenVehicleIsAlreadyRentedThrowsDomainException()
        {
            var vehicleRepository = new VehicleRepositoryStub();
            var rentalRepository = new RentalRepositoryStub();
            var outputPort = new RentVehicleOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new RentVehicleUseCase(vehicleRepository, rentalRepository, clock, new UnitOfWorkStub(), outputPort);
            var vehicle = CreateVehicle("VIN-001");
            vehicle.Rent();
            await vehicleRepository.Add(vehicle);

            Func<Task> act = () => useCase.Execute(new RentVehicleInput(vehicle.Id.Value, "12345678A", "Jane Doe"));

            await act.Should().ThrowAsync<VehicleAlreadyRentedException>();
            outputPort.StandardOutput.Should().BeNull();
            rentalRepository.Rentals.Should().BeEmpty();
        }

        private static Vehicle CreateVehicle(string vin)
        {
            return Vehicle.Create(
                Guid.NewGuid(),
                vin,
                "Toyota",
                "Corolla",
                new DateOnly(2023, 1, 10),
                new DateOnly(2026, 6, 7));
        }

        private sealed class ClockStub(DateTime currentUtcDateTime) : IClock
        {
            public DateOnly GetCurrentDate()
            {
                return DateOnly.FromDateTime(currentUtcDateTime);
            }

            public DateTime GetCurrentUtcDateTime()
            {
                return currentUtcDateTime;
            }
        }

        private sealed class RentVehicleOutputPortSpy : IRentVehicleOutputPort
        {
            public RentVehicleOutput StandardOutput { get; private set; }

            public string NotFoundMessage { get; private set; }

            public void StandardHandle(RentVehicleOutput response)
            {
                StandardOutput = response;
            }

            public void NotFoundHandle(string message)
            {
                NotFoundMessage = message;
            }
        }

        private sealed class UnitOfWorkStub : IUnitOfWork
        {
            public async Task Execute(Func<Task> operation)
            {
                await operation();
            }

            public Task<int> Save()
            {
                return Task.FromResult(0);
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

        private sealed class RentalRepositoryStub : IRentalRepository
        {
            public List<Rental> Rentals { get; } = new();

            public Task<bool> HasActiveRentalForPerson(string personDocumentId)
            {
                var hasActiveRental = Rentals.Exists(rental =>
                    rental.PersonDocumentId == personDocumentId && rental.Status == RentalStatus.Active);

                return Task.FromResult(hasActiveRental);
            }

            public Task Add(Rental rental)
            {
                Rentals.Add(rental);
                return Task.CompletedTask;
            }

            public Task<Rental> GetById(Guid rentalId)
            {
                return Task.FromResult(Rentals.Find(rental => rental.Id == rentalId));
            }

            public Task Update(Rental rental)
            {
                return Task.CompletedTask;
            }
        }
    }
}
