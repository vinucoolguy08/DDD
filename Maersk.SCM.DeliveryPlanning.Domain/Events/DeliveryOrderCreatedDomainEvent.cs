using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.Framework.Core.Common;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Events
{
    public class DeliveryOrderCreatedDomainEvent : IEvent, IDomainEvent
    {
        public DeliveryOrder DeliveryOrder { get; }

        public Guid Id { get; }

        public int Version { get; set; }

        public string TraceId { get; private set; }

        public IDomainEvent AssociatedDomainEvent => this;

        public DeliveryOrderCreatedDomainEvent(DeliveryOrder deliveryOrder, Guid id)
        {
            DeliveryOrder = deliveryOrder;
            Id = id;
        }
    }
}
