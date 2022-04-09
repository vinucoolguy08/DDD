using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators;
using Maersk.SCM.Framework.Core.Common;
using System;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices
{
    public class DeliveryPlanFactory : IDeliveryPlanFactory
    {
        private readonly ILegValidatorConfiguration _legValidatorConfiguration;

        public DeliveryPlanFactory(ILegValidatorConfiguration legValidatorConfiguration)
        {
            _legValidatorConfiguration = legValidatorConfiguration;
        }

        public IDeliveryPlan Create(
            long cargoStuffingId, 
            string containerType, 
            string containerReference, 
            string vesselName, 
            string vesselType)
        {
            var vessel = new Vessel(vesselName, vesselType);
            var shipment = new Shipment(containerType, containerReference, vessel);

            return new DeliveryPlan(Guid.NewGuid(), cargoStuffingId, shipment, BookingStatus.Created, _legValidatorConfiguration);
        }

        public IDeliveryPlan Create(IEnumerable<IEvent> events)
        {
            return new DeliveryPlan(events, _legValidatorConfiguration);
        }
    }
}
