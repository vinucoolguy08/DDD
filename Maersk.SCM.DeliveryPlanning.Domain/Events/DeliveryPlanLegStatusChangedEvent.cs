using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Events
{
    public class DeliveryPlanLegStatusChangedEvent : IEvent
    {
        public Guid DeliveryPlanLegId { get; private set; }

        public LegStatus OldStatus { get; private set; }

        public LegStatus NewStatus { get; private set; }

        public Guid Id { get; private set; }

        public int Version { get; set; }

        public string TraceId { get; private set; }

        public IDomainEvent AssociatedDomainEvent { get; private set; }

        public DeliveryPlanLegStatusChangedEvent(
            DeliveryPlan deliveryPlan, 
            Guid deliveryPlanLegId, 
            LegStatus oldStatus, 
            LegStatus newStatus, 
            Guid deliveryPlanId)
        {
            DeliveryPlanLegId = deliveryPlanLegId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Id = deliveryPlanId;

            AssociatedDomainEvent = new DeliveryPlanUpdatedDomainEvent(deliveryPlan);
        }
    }
}
