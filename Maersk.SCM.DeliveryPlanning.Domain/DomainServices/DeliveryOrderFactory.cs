using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices
{
    public class DeliveryOrderFactory : IDeliveryOrderFactory
    {
        public IDeliveryOrder Create(Guid deliveryPlanId, string providerCode)
        {
            return new DeliveryOrder(Guid.NewGuid(), deliveryPlanId, providerCode, 1, BookingStatus.Created);
        }
    }
}
