using Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents.Common;
using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.Framework.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents
{
    public record DeliveryOrderCreatedIntegrationEvent : IntegrationEvent
    {
        public static DeliveryOrderCreatedIntegrationEvent ToIntegrationEvent(IDeliveryOrder deliveryOrder)
        {
            return new DeliveryOrderCreatedIntegrationEvent
            {
                DeliveryOrderId = deliveryOrder.DeliveryOrderId,
                DeliveryOrderNumber = deliveryOrder.DeliveryOrderNumber,
                DeliveryPlanId = deliveryOrder.DeliveryPlanId,
                Status = new StatusIntegrationEvent
                { 
                    Id = deliveryOrder.Status.Id,
                    Name = deliveryOrder.Status.Name,
                },
                Legs = deliveryOrder.Legs.Select(x => new LegIntegrationEvent
                {
                    LegId = x.DeliveryOrderLegId,
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
                        SiteCode= x.EndLocation.SiteCode,
                        FullCountryName = x.EndLocation.FullCountryName,
                        FullSiteName= x.EndLocation.FullSiteName
                    }
                }).ToList()
            };
        }

        public Guid DeliveryOrderId { get; set; }

        public string DeliveryOrderNumber { get; set; }

        public Guid DeliveryPlanId { get; set; }

        public StatusIntegrationEvent Status { get; set; }

        public List<LegIntegrationEvent> Legs { get; set; }
    }
}
