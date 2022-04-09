using Maersk.SCM.Framework.Core.Persistence;
using System;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.Repositories
{
    public record DeliveryOrderEventSourceItems : EventSourcedItem
    {
        private DeliveryOrderEventSourceItems() : base()
        {
        }

        public DeliveryOrderEventSourceItems(Guid id, int version, object data, string eventName) :
            base(id, version, data, eventName)
        {

        }
    }
}
