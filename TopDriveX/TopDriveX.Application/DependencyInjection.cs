using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Services;

namespace TopDriveX.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddScoped<IMakeService, MakeService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ILogoService, LogoService>();

            return services;
        }
    }
}
