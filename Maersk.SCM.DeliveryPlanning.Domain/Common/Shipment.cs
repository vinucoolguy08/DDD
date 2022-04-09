using Maersk.SCM.Framework.Core.Common;
using System;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Common
{
    public class Shipment : ValueObject
    {
        public string ContainerType { get; private set; }

        public string ContainerReference { get; private set; }

        public Vessel Vessel { get; private set; }

        public Shipment(string containerType, string containerReference, Vessel vessel)
        {
            ContainerType = containerType;
            ContainerReference = containerReference;
            Vessel = vessel;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ContainerType;
            yield return ContainerReference;
            yield return Vessel;
        }

        public void UpdateVessel(Vessel vessel)
        {
            Vessel = vessel;
        }
    }
}
