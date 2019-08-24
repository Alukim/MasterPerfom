namespace MasterPerform.Infrastructure.Messaging.ServiceBus.Entities
{
    public class ServiceBusMessage
    {
        public ServiceBusMessage(string content, string type)
        {
            Content = content;
            Type = type;
        }

        public string Content { get; }
        public string Type { get; }
    }
}
