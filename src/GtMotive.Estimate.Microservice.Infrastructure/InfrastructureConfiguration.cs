using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Logging;
using GtMotive.Estimate.Microservice.Infrastructure.Rentals;
using GtMotive.Estimate.Microservice.Infrastructure.Telemetry;
using GtMotive.Estimate.Microservice.Infrastructure.Time;
using GtMotive.Estimate.Microservice.Infrastructure.Vehicles;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        [ExcludeFromCodeCoverage]
        public static IInfrastructureBuilder AddBaseInfrastructure(
            this IServiceCollection services,
            bool isDevelopment)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddSingleton<IClock, SystemClock>();
            services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
            services.AddSingleton<IRentalRepository, InMemoryRentalRepository>();

            if (!isDevelopment)
            {
                services.AddScoped<ITelemetry, AppTelemetry>();
            }
            else
            {
                services.AddScoped<ITelemetry, NoOpTelemetry>();
            }

            return new InfrastructureBuilder(services);
        }

        private sealed class InfrastructureBuilder(IServiceCollection services) : IInfrastructureBuilder
        {
            public IServiceCollection Services { get; } = services;
        }
    }
}
