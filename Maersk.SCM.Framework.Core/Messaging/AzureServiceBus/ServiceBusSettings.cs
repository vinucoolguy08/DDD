using System.Collections.Generic;

namespace Maersk.SCM.Framework.Core.Messaging.AzureServiceBus
{
    public class ServiceBusSettings
    {
        public ServiceBusSettings()
        {
            Clients = new List<ServiceBusClient>();
        }

        public string ConnectionString { get; set; }

        public List<ServiceBusClient> Clients { get; set; }
    }

    public class ServiceBusClient
    {
        public string ConnectionString { get; set; }

        public string Name { get; set; }

        public string TopicName { get; set; }

        public string SubscriptionName { get; set; }
    }
}
