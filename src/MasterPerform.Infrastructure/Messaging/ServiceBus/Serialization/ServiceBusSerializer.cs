using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace MasterPerform.Infrastructure.Messaging.ServiceBus.Serialization
{
    public class ServiceBusSerializer
    {
        private readonly JsonSerializerSettings settings;

        public ServiceBusSerializer()
            => settings = JsonSerializerSettingsProvider.CreateSerializerSettings();

        public string Serialize<TMessage>(TMessage message)
            => JsonConvert.SerializeObject(message, settings);

        public static ServiceBusSerializer Instance => new ServiceBusSerializer();
    }
}
