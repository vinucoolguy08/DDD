using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices
{
    public interface IDeliveryOrderFactory
    {
        public IDeliveryOrder Create(Guid deliveryPlanId, string providerCode);
    }
}
