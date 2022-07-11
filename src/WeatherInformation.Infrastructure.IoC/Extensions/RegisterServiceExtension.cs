using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherInformation.Infrastructure.IoC.Contracts;

namespace WeatherInformation.Infrastructure.IoC.Extensions
{
    public static class RegisterServiceExtension
    {
        public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appServices = typeof(RegisterServiceExtension).Assembly.DefinedTypes
                                .Where(x => typeof(IServiceRegistration)
                                .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                .Select(Activator.CreateInstance)
                                .Cast<IServiceRegistration>();


            //Register all application services that implement IServiceRegistration
            foreach (var service in appServices) service.RegisterAppServices(services, configuration);
        }
    }
}
