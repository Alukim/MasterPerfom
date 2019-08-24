using MasterPerform.Infrastructure.Messaging.ServiceBus.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.Infrastructure.Messaging.ServiceBus
{
    public static class ServiceBusBootstrapExtensions
    {
        public static IServiceCollection RegisterServiceBusSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
            => serviceCollection.Configure<ServiceBusSettings>(x => configuration.GetSection(nameof(ServiceBusSettings)).Bind(x));

        public static IServiceCollection AddServiceBusSender(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<ServiceBusSender>()
                .AddSingleton(ServiceBusSerializer.Instance);
        } 
    }
}
