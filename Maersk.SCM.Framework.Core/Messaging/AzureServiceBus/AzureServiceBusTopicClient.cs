using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Messaging.AzureServiceBus
{
    public class AzureServiceBusTopicClient : IMessageBroker
    {
        private ServiceBusSender _sender;
        private Azure.Messaging.ServiceBus.ServiceBusClient _client;
        private readonly string _topicName;
        private readonly string _subscriptionName;
        private readonly IServiceProvider _serviceProvider;
        private ServiceBusProcessor _processor;

        public AzureServiceBusTopicClient(string connectionString, string topicName, string subscriptionName, IServiceProvider serviceProvider)
        {
            _client = new Azure.Messaging.ServiceBus.ServiceBusClient(connectionString);
            _topicName = topicName;
            _subscriptionName = subscriptionName;
            _sender = _client.CreateSender(_topicName);
            _serviceProvider = serviceProvider;
        }

        public async Task<string> PublishAsync(IntegrationEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            var serviceBusMessage = new ServiceBusMessage(message)
            {
                MessageId = Guid.NewGuid().ToString()
            };

            await _sender.SendMessageAsync(serviceBusMessage);

            return serviceBusMessage.MessageId;
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _processor = _client.CreateProcessor(_topicName, _subscriptionName, new ServiceBusProcessorOptions());
            _processor.ProcessMessageAsync += MessageHandler<T, TH>;
            _processor.ProcessErrorAsync += ErrorHandler;
            _processor.StartProcessingAsync().GetAwaiter().GetResult();
        }

        private async Task MessageHandler<T, TH>(ProcessMessageEventArgs args)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var integrationEvent = JsonConvert.DeserializeObject<T>(args.Message.Body.ToString());

                var handler = scope.ServiceProvider.GetRequiredService<TH>();
                await (Task)typeof(TH).GetMethod(nameof(IIntegrationEventHandler<T>.Handle)).Invoke(handler, new object[] { integrationEvent });

                await args.CompleteMessageAsync(args.Message);
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _sender.DisposeAsync().GetAwaiter().GetResult();
            _processor.StopProcessingAsync().GetAwaiter().GetResult();
            _processor.DisposeAsync().GetAwaiter().GetResult();
            _client.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
