using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;
using Newtonsoft.Json;
using System;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate
{
    public class DeliveryPlanLeg
    {
        public Guid DeliveryLegId { get; private set; }

        public string ProviderCode { get; private set; }

        public DateTime PickUpDate { get; private set; }

        public DateTime DropOffDate { get; private set; }

        public Location StartLocation { get; private set; }

        public Location EndLocation { get; private set; }

        public LegStatus Status { get; private set; }

        public int Sequence { get; private set; }

        public bool HasDetailsChanged { get; private set; }

        public bool HasStatusChanged { get; private set; }

        [JsonConstructor]
        private DeliveryPlanLeg()
        {

        }

        public DeliveryPlanLeg(string providerCode, DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation, LegStatus status)
            :this(Guid.NewGuid(), providerCode, pickUpDate, dropOffDate, startLocation, endLocation, status)
        {
        }

        public DeliveryPlanLeg(Guid deliveryPlanLegId, string providerCode, DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation, LegStatus status)
        {
            DeliveryLegId = deliveryPlanLegId;
            ProviderCode = providerCode;
            PickUpDate = pickUpDate.VerifyOrThrowException(nameof(pickUpDate));
            DropOffDate = dropOffDate.VerifyOrThrowException(nameof(dropOffDate));
            StartLocation = startLocation.VerifyOrThrowException(nameof(startLocation));
            EndLocation = endLocation.VerifyOrThrowException(nameof(endLocation));
            Status = status;
        }

        public void UpdateDetails(DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation)
        {
            HasDetailsChanged = PickUpDate != pickUpDate || DropOffDate != dropOffDate || !StartLocation.Equals(startLocation) || !EndLocation.Equals(endLocation);

            PickUpDate = pickUpDate;
            DropOffDate = dropOffDate;
            StartLocation = startLocation;
            EndLocation = endLocation;
        }

        public void UpdateStatus(LegStatus status)
        {
            HasStatusChanged = !Status.Equals(status);

            Status = status;
        }

        public void SetSequence(int sequence)
        {
            Sequence = sequence;
        }
    }
}
