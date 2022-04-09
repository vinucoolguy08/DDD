using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.DeliveryPlanning.Domain.Events;
using Maersk.SCM.Framework.Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate
{
    public class DeliveryOrder : Entity, IDeliveryOrder, IAggregateRoot
    {
        [JsonProperty(PropertyName = "Legs")]
        private List<DeliveryOrderLeg> _legs;

        public Guid DeliveryOrderId { get; private set; }

        public int Version { get; private set; }

        public string DeliveryOrderNumber { get; private set; }

        public Guid DeliveryPlanId { get; private set; }

        public BookingStatus Status { get; private set; }

        [JsonIgnore]
        public IReadOnlyCollection<DeliveryOrderLeg> Legs => _legs.AsReadOnly();

        public string ProviderCode { get; private set; }

        // Required for Entity Framework
        private DeliveryOrder()
        {
            _legs = new List<DeliveryOrderLeg>();
        }

        public DeliveryOrder(Guid deliveryOrderId, Guid deliveryPlanId, string providerCode, int version, BookingStatus status) : this()
        {
            DeliveryOrderId = deliveryOrderId;
            DeliveryPlanId = deliveryPlanId;
            Version = version;
            DeliveryOrderId = deliveryOrderId.VerifyOrThrowException(nameof(DeliveryOrderId));
            ProviderCode = providerCode.VerifyOrThrowException(nameof(ProviderCode));
            Status = status;

            AddEvent(new DeliveryOrderCreatedDomainEvent(this, DeliveryOrderId));
        }

        public void AddLeg(DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation)
        {
            _legs.Add(new DeliveryOrderLeg(pickUpDate, dropOffDate, startLocation, endLocation, LegStatus.Created));
        }

        public void SetDeliveryOrderNumber(string deliveryOrderNumber)
        {
            DeliveryOrderNumber = deliveryOrderNumber;
        }
    }
}
