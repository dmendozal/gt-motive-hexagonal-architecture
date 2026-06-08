using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental;
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
    public sealed class ReturnRentalUseCaseTests
    {
        [Fact]
        public async Task ExecuteWhenRentalIsActiveAndBelongsToPersonReturnsRentalAndMakesVehicleAvailable()
        {
            var rentalRepository = new RentalRepositoryStub();
            var vehicleRepository = new VehicleRepositoryStub();
            var outputPort = new ReturnRentalOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new ReturnRentalUseCase(rentalRepository, vehicleRepository, clock, new UnitOfWorkStub(), outputPort);
            var vehicle = CreateVehicle("VIN-001");
            vehicle.Rent();
            await vehicleRepository.Add(vehicle);
            var rental = Rental.Create(
                Guid.NewGuid(),
                vehicle.Id.Value,
                "12345678A",
                "Jane Doe",
                clock.GetCurrentUtcDateTime());
            await rentalRepository.Add(rental);

            await useCase.Execute(new ReturnRentalInput(rental.Id, "12345678A"));

            outputPort.StandardOutput.Should().NotBeNull();
            outputPort.StandardOutput.RentalId.Should().Be(rental.Id);
            rental.Status.Should().Be(RentalStatus.Returned);
            vehicle.Status.Should().Be(VehicleStatus.Available);
        }

        [Fact]
        public async Task ExecuteWhenRentalDoesNotExistReturnsNotFound()
        {
            var rentalRepository = new RentalRepositoryStub();
            var vehicleRepository = new VehicleRepositoryStub();
            var outputPort = new ReturnRentalOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new ReturnRentalUseCase(rentalRepository, vehicleRepository, clock, new UnitOfWorkStub(), outputPort);

            await useCase.Execute(new ReturnRentalInput(Guid.NewGuid(), "12345678A"));

            outputPort.NotFoundMessage.Should().Be("Rental not found.");
            outputPort.StandardOutput.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteWhenRentalIsAlreadyReturnedThrowsDomainException()
        {
            var rentalRepository = new RentalRepositoryStub();
            var vehicleRepository = new VehicleRepositoryStub();
            var outputPort = new ReturnRentalOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new ReturnRentalUseCase(rentalRepository, vehicleRepository, clock, new UnitOfWorkStub(), outputPort);
            var rental = Rental.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "12345678A",
                "Jane Doe",
                clock.GetCurrentUtcDateTime());
            rental.Return("12345678A", clock.GetCurrentUtcDateTime());
            await rentalRepository.Add(rental);

            Func<Task> act = () => useCase.Execute(new ReturnRentalInput(rental.Id, "12345678A"));

            await act.Should().ThrowAsync<RentalAlreadyReturnedException>();
            outputPort.StandardOutput.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteWhenRentalBelongsToAnotherPersonThrowsDomainException()
        {
            var rentalRepository = new RentalRepositoryStub();
            var vehicleRepository = new VehicleRepositoryStub();
            var outputPort = new ReturnRentalOutputPortSpy();
            var clock = new ClockStub(new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc));
            var useCase = new ReturnRentalUseCase(rentalRepository, vehicleRepository, clock, new UnitOfWorkStub(), outputPort);
            var rental = Rental.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "12345678A",
                "Jane Doe",
                clock.GetCurrentUtcDateTime());
            await rentalRepository.Add(rental);

            Func<Task> act = () => useCase.Execute(new ReturnRentalInput(rental.Id, "87654321B"));

            await act.Should().ThrowAsync<RentalDoesNotBelongToPersonException>();
            outputPort.StandardOutput.Should().BeNull();
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

        private sealed class ReturnRentalOutputPortSpy : IReturnRentalOutputPort
        {
            public ReturnRentalOutput StandardOutput { get; private set; }

            public string NotFoundMessage { get; private set; }

            public void StandardHandle(ReturnRentalOutput response)
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
            private readonly List<Rental> rentals = new();

            public Task<bool> HasActiveRentalForPerson(string personDocumentId)
            {
                var hasActiveRental = rentals.Exists(rental =>
                    rental.PersonDocumentId == personDocumentId && rental.Status == RentalStatus.Active);

                return Task.FromResult(hasActiveRental);
            }

            public Task Add(Rental rental)
            {
                rentals.Add(rental);
                return Task.CompletedTask;
            }

            public Task<Rental> GetById(Guid rentalId)
            {
                return Task.FromResult(rentals.Find(rental => rental.Id == rentalId));
            }

            public Task Update(Rental rental)
            {
                return Task.CompletedTask;
            }
        }
    }
}
