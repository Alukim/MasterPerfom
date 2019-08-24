namespace MasterPerform.Infrastructure.Messaging.ServiceBus
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public bool ShouldForward { get; set; } = false;
    }
}
