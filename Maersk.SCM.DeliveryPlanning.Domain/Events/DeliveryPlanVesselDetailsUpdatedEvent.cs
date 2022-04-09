using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Events
{
    public class DeliveryPlanVesselDetailsUpdatedEvent : IEvent
    {
        public Vessel Vessel { get; private set; }

        public IDomainEvent AssociatedDomainEvent { get; private set; }

        public Guid Id { get; private set; }

        public int Version { get; set; }

        public string TraceId { get; private set; }

        public DeliveryPlanVesselDetailsUpdatedEvent(DeliveryPlan deliveryPlan, Vessel vessel, Guid deliveryPlanId)
        {
            Vessel = vessel;
            Id = deliveryPlanId;

            AssociatedDomainEvent = new DeliveryPlanUpdatedDomainEvent(deliveryPlan);
        }
    }
}
