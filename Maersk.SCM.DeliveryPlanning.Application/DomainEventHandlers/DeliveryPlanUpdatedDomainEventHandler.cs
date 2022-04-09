using Maersk.SCM.DeliveryPlanning.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.DomainEventHandlers
{
    public class DeliveryPlanUpdatedDomainEventHandler : INotificationHandler<DeliveryPlanUpdatedDomainEvent>
    {
        public Task Handle(DeliveryPlanUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Do something 

            return Task.FromResult(0);
        }
    }
}
