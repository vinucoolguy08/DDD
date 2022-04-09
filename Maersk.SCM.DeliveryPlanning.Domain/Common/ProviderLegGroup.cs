using System;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Common
{
    public class ProviderLegGroup
    {
        public string ProviderCode { get; set; }

        public IEnumerable<ProviderLeg> Legs { get; set; }

        public ProviderLegGroup(string providerCode, IEnumerable<ProviderLeg> legs)
        {
            ProviderCode = providerCode;
            Legs = legs;
        }
    }

    public class ProviderLeg
    {
        public DateTime PickUpDate { get; set; }

        public DateTime DropOffDate { get; set; }

        public Location StartLocation { get; set; }

        public Location EndLocation { get; set; }
    }
}
