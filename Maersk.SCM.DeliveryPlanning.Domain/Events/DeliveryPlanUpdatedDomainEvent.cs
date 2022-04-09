using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.Framework.Core.Common;

namespace Maersk.SCM.DeliveryPlanning.Domain.Events
{
    public class DeliveryPlanUpdatedDomainEvent : IDomainEvent
    {
        public DeliveryPlan DeliveryPlan { get; private set; }


        public DeliveryPlanUpdatedDomainEvent(DeliveryPlan deliveryPlan)
        {
            DeliveryPlan = deliveryPlan;
        }
    }
}
