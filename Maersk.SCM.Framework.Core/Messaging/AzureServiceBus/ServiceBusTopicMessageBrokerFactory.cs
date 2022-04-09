using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Maersk.SCM.Framework.Core.Messaging.AzureServiceBus
{
    public class ServiceBusTopicMessageBrokerFactory : IMessageBrokerFactory
    {
        private Dictionary<string, IMessageBroker> _messageBrokers;
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";

        public ServiceBusTopicMessageBrokerFactory(
            IOptions<ServiceBusSettings> options,
            IServiceProvider serviceProvider,
            ILogger<ServiceBusTopicMessageBrokerFactory> logger)
        {
            _messageBrokers = new Dictionary<string, IMessageBroker>();

            foreach (var clientSetting in options.Value.Clients)
            {
                var connectionString = clientSetting.ConnectionString ?? options.Value.ConnectionString;
                _messageBrokers.Add(clientSetting.Name,
                    new AzureServiceBusTopicClient(connectionString, clientSetting.TopicName, clientSetting.SubscriptionName, serviceProvider));
            }
        }

        public IMessageBroker GetBrokerClient(string clientName)
        {
            if (!_messageBrokers.ContainsKey(clientName))
            {
                throw new Exception($"Client Name {clientName} is not registered in the ServiceBusMessageBrokerFactory");
            }

            return _messageBrokers[clientName];
        }

        public IMessageBroker GetBrokerClient<T>() where T : IntegrationEvent
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

            return GetBrokerClient(eventName);
        }
    }
}
