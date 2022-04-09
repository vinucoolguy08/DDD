using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.Framework.Core.Common;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Events
{
    public record DeliveryPlanCreatedEvent : IEvent, IDomainEvent
    {
        public DeliveryPlan DeliveryPlan { get; private set; }

        public Guid Id { get; private set; }

        public int Version { get; set; }

        public string TraceId { get; private set; }

        public IDomainEvent AssociatedDomainEvent => this;

        public DeliveryPlanCreatedEvent(DeliveryPlan deliveryPlan, Guid id)
        {
            DeliveryPlan = deliveryPlan;
            Id = id;
        }
    }
}
