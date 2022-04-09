using Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents.Common;
using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.Framework.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents
{
    public record DeliveryPlanCreatedIntegrationEvent : IntegrationEvent
    {
        // This should go to a mapper class
        public static DeliveryPlanCreatedIntegrationEvent ToIntegrationEvent(IDeliveryPlan deliveryPlan)
        {
            return new DeliveryPlanCreatedIntegrationEvent
            {
                Id = Guid.NewGuid(),
                DeliveryPlanId = deliveryPlan.DeliveryPlanId,
                CargoStuffingId = deliveryPlan.CargoStuffingId,
                Shipment = new ShipmentIntegrationEvent
                {
                    ContainerType = deliveryPlan.Shipment.ContainerType,
                    ContainerReference = deliveryPlan.Shipment.ContainerReference,
                    Vessel = new VesselIntegrationEvent
                    {
                        Type = deliveryPlan.Shipment.Vessel.Type,
                        Name = deliveryPlan.Shipment.Vessel.Name
                    }
                },
                Status = new StatusIntegrationEvent
                {
                    Id = deliveryPlan.Status.Id,
                    Name = deliveryPlan.Status.Name,
                },
                Legs = deliveryPlan.Legs.Select(x => new LegIntegrationEvent
                {
                    LegId = x.DeliveryLegId,
                    ProviderCode = x.ProviderCode,
                    PickUpDate = x.PickUpDate,
                    DropOffDate = x.DropOffDate,
                    StartLocation = new LocationIntegrationEvent
                    {
                        CountryCode = x.StartLocation.CountryCode,
                        SiteCode = x.StartLocation.SiteCode,
                        FullCountryName = x.StartLocation.FullCountryName,
                        FullSiteName = x.StartLocation.FullSiteName
                    },
                    EndLocation = new LocationIntegrationEvent
                    {
                        CountryCode = x.EndLocation.CountryCode,
                        SiteCode = x.EndLocation.SiteCode,
                        FullCountryName = x.EndLocation.FullCountryName,
                        FullSiteName = x.EndLocation.FullSiteName
                    },
                    Status = new StatusIntegrationEvent
                    {
                        Id = x.Status.Id,
                        Name= x.Status.Name,
                    },
                    Sequence = x.Sequence
                }).ToList(),
                CreatedBy = "SomeUserFromAuthToken",
                CreationDate = DateTime.UtcNow
            };
        }

        public Guid DeliveryPlanId { get; set; }

        public long CargoStuffingId { get; set; }

        public ShipmentIntegrationEvent Shipment { get; set; }

        public StatusIntegrationEvent Status { get; set; }

        public List<LegIntegrationEvent> Legs { get; set; }
    }

    public class ShipmentIntegrationEvent
    {
        public string ContainerType { get; set; }

        public string ContainerReference { get; set; }

        public VesselIntegrationEvent Vessel { get; set; }
    }

    public class VesselIntegrationEvent
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }
}
