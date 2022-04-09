using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Maersk.SCM.Framework.Core.Messaging.AzureServiceBus
{
    public static class ServiceBusMessageBrokerExtensions
    {
        public static IServiceCollection AddServiceBusTopicMessageBrokerFactory(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceBusSettings>(configuration.GetSection(nameof(ServiceBusSettings)));
            return services.AddSingleton<IMessageBrokerFactory, ServiceBusTopicMessageBrokerFactory>();
        }

        public static IServiceCollection AddSingletonIntegrationEventHandler<TEventModel, TEventHandler>(this IServiceCollection services)
            where TEventModel : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEventModel>
        {
            return services.AddSingleton(typeof(IIntegrationEventHandler<TEventModel>), typeof(TEventHandler));
        }

        public static void UseIntegrationEventHandlerSubscriber<TEventModel>(this IApplicationBuilder app)
            where TEventModel : IntegrationEvent
        {
            var messageBrokerFactory = app.ApplicationServices.GetRequiredService<IMessageBrokerFactory>();

            messageBrokerFactory.GetBrokerClient<TEventModel>()
                .Subscribe<TEventModel, IIntegrationEventHandler<TEventModel>>();
        }
    }
}
