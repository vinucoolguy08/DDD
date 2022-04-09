using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Messaging
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
           where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
