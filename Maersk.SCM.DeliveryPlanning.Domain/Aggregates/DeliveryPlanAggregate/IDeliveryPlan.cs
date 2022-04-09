using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;
using System;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate
{
    public interface IDeliveryPlan: IEntity
    {
        Guid DeliveryPlanId { get; }

        long CargoStuffingId { get; }

        Shipment Shipment { get; }

        BookingStatus Status { get; }

        IReadOnlyCollection<DeliveryPlanLeg> Legs { get; }

        void AddLeg(string providerCode, DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation);

        void UpdateLeg(Guid deliveryPlanLegId, DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation, LegStatus status);

        void UpdateVesselDetails(string name, string type);

        void RemoveLegsNotInList(IEnumerable<Guid> deliveryPlanLegIds);
    }
}
