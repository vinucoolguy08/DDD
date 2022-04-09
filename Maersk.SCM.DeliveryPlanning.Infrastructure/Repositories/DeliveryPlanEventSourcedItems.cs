using Maersk.SCM.Framework.Core.Persistence;
using System;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.Repositories
{
    public record DeliveryPlanEventSourcedItems : EventSourcedItem
    {
        private DeliveryPlanEventSourcedItems() : base()
        {
        }

        public DeliveryPlanEventSourcedItems(Guid id, int version, object data, string eventName) :
            base(id, version, data, eventName)
        {

        }
    }
}
