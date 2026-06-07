using GtMotive.Estimate.Microservice.Api.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.Api.Rentals.ReturnRental;
using GtMotive.Estimate.Microservice.Api.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.ReturnRental;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.GetAvailableVehicles;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static void AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<CreateVehiclePresenter>();
            services.AddScoped<ICreateVehicleOutputPort>(
                static provider => provider.GetRequiredService<CreateVehiclePresenter>());
            services.AddScoped<GetAvailableVehiclesPresenter>();
            services.AddScoped<IGetAvailableVehiclesOutputPort>(
                static provider => provider.GetRequiredService<GetAvailableVehiclesPresenter>());
            services.AddScoped<RentVehiclePresenter>();
            services.AddScoped<IRentVehicleOutputPort>(static provider => provider.GetRequiredService<RentVehiclePresenter>());
            services.AddScoped<ReturnRentalPresenter>();
            services.AddScoped<IReturnRentalOutputPort>(
                static provider => provider.GetRequiredService<ReturnRentalPresenter>());
        }
    }
}
