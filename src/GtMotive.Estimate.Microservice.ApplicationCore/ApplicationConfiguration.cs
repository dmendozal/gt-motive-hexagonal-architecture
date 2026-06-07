using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.ApplicationCore
{
    /// <summary>
    /// Adds Use Cases classes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Adds Use Cases to the ServiceCollection.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>The modified instance.</returns>
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<CreateVehicleUseCase>();
            services.AddScoped<GetAvailableVehiclesUseCase>();
            services.AddScoped<RentVehicleUseCase>();
            services.AddScoped<ReturnRentalUseCase>();

            return services;
        }
    }
}
