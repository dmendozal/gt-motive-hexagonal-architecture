using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles.AggregateRoots;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Enums;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain
{
    public sealed class VehicleTests
    {
        [Fact]
        public void CreateWhenManufacturingDateExceedsFiveYearsReturnsFailure()
        {
            var currentDate = new DateOnly(2026, 6, 7);
            var manufacturingDate = new DateOnly(2021, 6, 6);

            Action act = () => Vehicle.Create(
                Guid.NewGuid(),
                "VIN-001",
                "Toyota",
                "Corolla",
                manufacturingDate,
                currentDate);

            act.Should().Throw<VehicleManufacturingDateExceededException>();
        }

        [Fact]
        public void CreateWhenManufacturingDateIsExactlyFiveYearsOldReturnsSuccess()
        {
            var currentDate = new DateOnly(2026, 6, 7);
            var manufacturingDate = new DateOnly(2021, 6, 7);

            var vehicle = Vehicle.Create(
                Guid.NewGuid(),
                "VIN-001",
                "Toyota",
                "Corolla",
                manufacturingDate,
                currentDate);

            vehicle.Status.Should().Be(VehicleStatus.Available);
        }
    }
}
