using Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents;
using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using Maersk.SCM.DeliveryPlanning.Domain.Events;
using Maersk.SCM.Framework.Core.Messaging;
using Maersk.SCM.Framework.Core.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.DomainEventHandlers
{
    public class DeliveryPlanCreatedDomainEventHandler : INotificationHandler<DeliveryPlanCreatedEvent>
    {
        private readonly IMessageBroker _deliveryOrderMessageBroker;
        private readonly IEventSourcedRepository<IDeliveryOrder> _eventSourcedRepository;
        private readonly IDeliveryOrderFactory _deliveryOrderFactory;
        private readonly IDeliveryOrderNumberGenerator _deliveryOrderNumberGenerator;

        public DeliveryPlanCreatedDomainEventHandler(
            IMessageBrokerFactory messageBrokerFactory, 
            IEventSourcedRepository<IDeliveryOrder> eventSourcedRepository,
            IDeliveryOrderFactory deliveryOrderFactory,
            IDeliveryOrderNumberGenerator deliveryOrderNumberGenerator)
        {
            _deliveryOrderMessageBroker = messageBrokerFactory.GetBrokerClient("DeliveryOrderCreated");
            _eventSourcedRepository = eventSourcedRepository;
            _deliveryOrderFactory = deliveryOrderFactory;
            _deliveryOrderNumberGenerator = deliveryOrderNumberGenerator;
        }

        public async Task Handle(DeliveryPlanCreatedEvent notification, CancellationToken cancellationToken)
        {
            var providerGroupLegs = notification.DeliveryPlan.GetLegsGroupedByProviderCode().ToList();

            // From one Delivery Plan create one or more Delivery Orders
            foreach (var providerLegs in providerGroupLegs)
            {
                var deliveryOrder = _deliveryOrderFactory.Create(notification.DeliveryPlan.DeliveryPlanId, providerLegs.ProviderCode);
                var deliveryOrderNumber = await _deliveryOrderNumberGenerator.GenerateOrderNumber(new DeliveryOrderReference(deliveryOrder.DeliveryOrderId));
                deliveryOrder.SetDeliveryOrderNumber(deliveryOrderNumber);

                foreach (var leg in providerLegs.Legs)
                {
                    deliveryOrder.AddLeg(leg.PickUpDate, leg.DropOffDate, (Location)leg.StartLocation.ShallowCopy(), (Location)leg.EndLocation.ShallowCopy());
                }

                await _eventSourcedRepository.SaveAsync(deliveryOrder);
                await _deliveryOrderMessageBroker.PublishAsync(DeliveryOrderCreatedIntegrationEvent.ToIntegrationEvent(deliveryOrder));
            }
        }
    }
}
