using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.Framework.Core.Common;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Events
{
    public class DeliveryPlanLegRemovedEvent : IEvent
    {
        public DeliveryPlanLeg DeliveryPlanLeg { get; private set; }

        public Guid Id { get; private set; }

        public int Version { get; set; }

        public string TraceId { get; private set; }

        public IDomainEvent AssociatedDomainEvent { get; private set; }

        public DeliveryPlanLegRemovedEvent(DeliveryPlan deliveryPlan, DeliveryPlanLeg deliveryPlanLeg, Guid deliveryPlanId)
        {
            DeliveryPlanLeg = deliveryPlanLeg;
            Id = deliveryPlanId;

            AssociatedDomainEvent = new DeliveryPlanUpdatedDomainEvent(deliveryPlan);
        }
    }
}
