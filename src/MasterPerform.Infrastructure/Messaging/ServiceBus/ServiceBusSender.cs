using MasterPerform.Infrastructure.EnvironmentPrefixer;
using MasterPerform.Infrastructure.Messaging.ServiceBus.Entities;
using MasterPerform.Infrastructure.Messaging.ServiceBus.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.Messaging.ServiceBus
{
    public class ServiceBusSender
    {
        private readonly ServiceBusSettings _settings;
        private readonly IEnvironmentPrefixer _prefixer;
        private readonly ServiceBusSerializer _serializer;

        public ServiceBusSender(IEnvironmentPrefixer prefixer, IOptions<ServiceBusSettings> settings, ServiceBusSerializer serializer)
        {
            this._prefixer = prefixer;
            this._serializer = serializer;
            this._settings = settings.Value;
        }

        public async Task SendMessage<TMessage>(TMessage message)
        {
            if (!_settings.ShouldForward)
                return;

            var queueClient = GetQueueClient();
            var serviceBusMessage = BuildMessage(message);
            await queueClient.SendAsync(serviceBusMessage);
        }

        private QueueClient GetQueueClient()
        {
            var environmentQueue = _prefixer.AppendPrefix(_settings.QueueName);
            return new QueueClient(connectionString: _settings.ConnectionString, entityPath: environmentQueue);
        }

        private Message BuildMessage<TMessage>(TMessage message)
        {
            var content = _serializer.Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(
                content: content,
                type: message.GetType().Name);
            var data = _serializer.Serialize(serviceBusMessage);

            return new Message(Encoding.UTF8.GetBytes(data));
        }
    }
}
