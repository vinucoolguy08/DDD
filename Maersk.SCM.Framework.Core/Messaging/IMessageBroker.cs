using System;
using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        Task<string> PublishAsync(IntegrationEvent @event);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
