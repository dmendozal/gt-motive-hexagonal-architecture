using GtMotive.Estimate.Microservice.ApplicationCore.Ports;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Logging;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using GtMotive.Estimate.Microservice.Infrastructure.Rentals;
using GtMotive.Estimate.Microservice.Infrastructure.Time;
using GtMotive.Estimate.Microservice.Infrastructure.Vehicles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    internal static class MongoFunctionalInfrastructureConfiguration
    {
        public static IServiceCollection AddMongoFunctionalInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
            services.AddLogging(static logging => logging.AddConsole());
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddSingleton<IClock, SystemClock>();
            services.AddSingleton<MongoService>();
            services.AddSingleton<MongoSessionContext>();
            services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
            services.AddSingleton<IVehicleRepository, MongoVehicleRepository>();
            services.AddSingleton<IRentalRepository, MongoRentalRepository>();

            return services;
        }
    }
}
