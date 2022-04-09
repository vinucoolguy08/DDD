namespace Maersk.SCM.Framework.Core.Messaging
{
    public interface IMessageBrokerFactory
    {
        IMessageBroker GetBrokerClient(string clientName);

        IMessageBroker GetBrokerClient<T>() where T : IntegrationEvent;
    }
}
