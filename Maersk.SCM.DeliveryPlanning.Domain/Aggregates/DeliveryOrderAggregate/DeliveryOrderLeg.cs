using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate
{
    public class DeliveryOrderLeg
    {
        public Guid DeliveryOrderLegId { get; set; }

        public DateTime PickUpDate { get; private set; }

        public DateTime DropOffDate { get; private set; }

        public Location StartLocation { get; private set; }

        public Location EndLocation { get; private set; }

        public LegStatus Status { get; private set; }

        public DeliveryOrderLeg(DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation, LegStatus status)
        {
            DeliveryOrderLegId = Guid.NewGuid();
            PickUpDate = pickUpDate.VerifyOrThrowException(nameof(pickUpDate));
            DropOffDate = dropOffDate.VerifyOrThrowException(nameof(dropOffDate));
            StartLocation = startLocation.VerifyOrThrowException(nameof(startLocation));
            EndLocation = endLocation.VerifyOrThrowException(nameof(endLocation));
            Status = status.VerifyOrThrowException(nameof(status));
        }
    }
}
