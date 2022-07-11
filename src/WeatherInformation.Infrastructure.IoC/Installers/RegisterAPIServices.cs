using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using WeatherInformation.Application.Contracts;
using WeatherInformation.Application.Services;
using WeatherInformation.Infrastructure.Azure;
using WeatherInformation.Infrastructure.IoC.Contracts;

namespace WeatherInformation.Infrastructure.IoC.Installers
{
    public class RegisterAPIServices : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAzureBlobService>(_ =>
            {
                var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("AzureAccountStorage"));
                return new AzureBlobService(storageAccount);
            });

            services.AddScoped<IMeasurementDataService, MeasurementDataService>();
        }
    }
}
