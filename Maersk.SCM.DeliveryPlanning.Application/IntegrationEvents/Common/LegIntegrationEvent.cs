using System;

namespace Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents.Common
{
    public class LegIntegrationEvent
    {
        public Guid LegId { get; set; }

        public string ProviderCode { get; set; }

        public DateTime PickUpDate { get; set; }

        public DateTime DropOffDate { get; set; }

        public LocationIntegrationEvent StartLocation { get; set; }

        public LocationIntegrationEvent EndLocation { get; set; }

        public StatusIntegrationEvent Status { get; set; }

        public int Sequence { get; set; }
    }
}
