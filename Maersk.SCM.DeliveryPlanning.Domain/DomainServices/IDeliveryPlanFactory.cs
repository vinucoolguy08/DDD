using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.Framework.Core.Common;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices
{
    public interface IDeliveryPlanFactory
    {
        IDeliveryPlan Create(long cargoStuffingId,
            string shipmentType,
            string shipmentReference,
            string vesselName,
            string vesselType);

        IDeliveryPlan Create(IEnumerable<IEvent> events);
    }
}
