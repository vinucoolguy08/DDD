using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;
using System;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate
{
    public interface IDeliveryOrder : IEntity
    {
        public Guid DeliveryOrderId { get; }

        public int Version { get; }

        public string DeliveryOrderNumber { get; }

        public string ProviderCode { get; }

        public Guid DeliveryPlanId { get; }

        public BookingStatus Status { get; }

        public IReadOnlyCollection<DeliveryOrderLeg> Legs { get; }

        public void AddLeg(DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation);

        public void SetDeliveryOrderNumber(string deliveryOrderNumber);
    }
}
